using System;
using WpfMessageBox = System.Windows.MessageBox;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using AiriAssistant.Services;
using AiriAssistant.Managers;

namespace AiriAssistant
{
    public partial class MainWindow : Window
    {
        private readonly SoundService _soundService = new();
        private readonly ShutdownService _shutdownService;
        private readonly TrayIconManager _trayIconManager = new();
        private readonly HotkeyManager _hotkeyManager = new();
        private readonly PowerService _powerService;

        private IntPtr _windowHandle;

        public MainWindow()
        {
            InitializeComponent();

            _powerService = new PowerService();
            _shutdownService = new ShutdownService(_soundService, _powerService);

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Hide();

            _soundService.Play("Asset\\Sounds\\System\\startup.wav");

            _trayIconManager.Initialize();

            _trayIconManager.ShutdownClicked += async () =>
            {
                await _shutdownService.ShowShutdownConfirm();
            };

            _trayIconManager.ExitClicked += () =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            _windowHandle = new WindowInteropHelper(this).Handle;

            _hotkeyManager.HotkeyPressed += async () =>
            {
                await _shutdownService.ShowShutdownConfirm();
            };

            bool success = _hotkeyManager.Register(_windowHandle);

            if (!success)
            {
                System.Windows.MessageBox.Show("Không thể đăng ký hotkey Ctrl + Alt + S.");
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _hotkeyManager.Unregister();

            if (_trayIconManager != null)
            {
                {
                    _trayIconManager.Dispose();
                }
            }
        }

        private async void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            await _shutdownService.ShowShutdownConfirm();
        }
    }
}