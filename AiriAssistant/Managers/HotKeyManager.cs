using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace AiriAssistant.Managers
{
    public class HotkeyManager
    {
        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;

        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint VK_S = 0x53;

        private IntPtr _windowHandle;
        private HwndSource _source;

        public event Action HotkeyPressed;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(
            IntPtr hWnd,
            int id,
            uint fsModifiers,
            uint vk
        );

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id
        );

        public bool Register(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            return RegisterHotKey(
                _windowHandle,
                HOTKEY_ID,
                MOD_CONTROL | MOD_SHIFT | MOD_ALT,
                VK_S
            );
        }

        public void Unregister()
        {
            UnregisterHotKey(_windowHandle, HOTKEY_ID);

            if (_source != null)
            {
                _source.RemoveHook(HwndHook);
                _source = null;
            }
        }

        private IntPtr HwndHook(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled
        )
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                HotkeyPressed?.Invoke();
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}