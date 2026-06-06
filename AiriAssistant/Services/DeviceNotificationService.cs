using System;
using System.Linq;
using System.Windows.Interop;
using System.Management;

namespace AiriAssistant.Services
{
    public class DeviceNotificationService
    {
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVNODES_CHANGED = 0x0007;

        private readonly SoundService _soundService;
        private HwndSource? _source;

        private int _lastUsbDeviceCount;
        private DateTime _lastSoundTime = DateTime.MinValue;

        public DeviceNotificationService(SoundService soundService)
        {
            _soundService = soundService;
            _lastUsbDeviceCount = GetUsbDeviceCount();
        }

        public void Start(IntPtr windowHandle)
        {
            _source = HwndSource.FromHwnd(windowHandle);

            if (_source == null)
            {
                System.Windows.MessageBox.Show("Không thể lấy HwndSource cho DeviceNotificationService.");
                return;
            }

            _source.AddHook(WndProc);
        }

        public void Stop()
        {
            if (_source == null) return;

            _source.RemoveHook(WndProc);
            _source = null;
        }

        private IntPtr WndProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled
        )
        {
            if (msg == WM_DEVICECHANGE && wParam.ToInt32() == DBT_DEVNODES_CHANGED)
            {
                HandleDeviceChanged();
            }

            return IntPtr.Zero;
        }

        private void HandleDeviceChanged()
        {
            if ((DateTime.Now - _lastSoundTime).TotalMilliseconds < 1000)
                return;

            int currentCount = GetUsbDeviceCount();

            if (currentCount > _lastUsbDeviceCount)
            {
                _soundService.Play("Asset\\Sounds\\Device\\device_connected.wav");
                _lastSoundTime = DateTime.Now;
            }
            else if (currentCount < _lastUsbDeviceCount)
            {
                _soundService.Play("Asset\\Sounds\\Device\\device_disconnected.wav");
                _lastSoundTime = DateTime.Now;
            }

            _lastUsbDeviceCount = currentCount;
        }

        private int GetUsbDeviceCount()
        {
            try
            {
                using ManagementObjectSearcher searcher = new(
                    "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE 'USB%'"
                );

                return searcher.Get().Cast<ManagementObject>().Count();
            }
            catch
            {
                return _lastUsbDeviceCount;
            }
        }
    }
}