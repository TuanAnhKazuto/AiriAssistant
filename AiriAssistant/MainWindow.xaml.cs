using AiriAssistant.Managers;
using AiriAssistant.Services;
using System.Windows;
using System.Windows.Interop;
using WpfMessageBox = System.Windows.MessageBox;

namespace AiriAssistant
{
    public partial class MainWindow : Window
    {
        private readonly SoundService _soundService = new();
        private readonly ShutdownService _shutdownService;
        private readonly TrayIconManager _trayIconManager = new();
        private readonly HotkeyManager _hotkeyManager = new();
        private readonly PowerService _powerService = new();
        private readonly BatteryPowerService _batteryPowerService;
        //private readonly App _app = new();

        private IntPtr _windowHandle;

        public MainWindow()
        {
            InitializeComponent();

            _shutdownService = new(_soundService, _powerService);
            _batteryPowerService = new(_soundService);

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Hide();

            // Phát âm thanh khởi động
            //if (_app.IsRunning) return;
            _soundService.Play("Asset\\Sounds\\System\\startup.wav");


            // Bắt đầu theo dõi trạng thái pin
            _batteryPowerService.Start();

            // Khởi tạo và hiển thị icon trên system tray
            _trayIconManager.Initialize();

            _trayIconManager.ShutdownClicked += async () =>
            {
                await _shutdownService.ShowShutdownConfirm();
            };

            _trayIconManager.ExitClicked += () =>
            {
                System.Windows.Application.Current.Shutdown();
            };

            // Đăng ký hotkey
            _windowHandle = new WindowInteropHelper(this).Handle;

            _hotkeyManager.HotkeyPressed += async () =>
            {
                await _shutdownService.ShowShutdownConfirm();
            };

            bool success = _hotkeyManager.Register(_windowHandle);

            if (!success)
            {
                WpfMessageBox.Show("Không thể đăng ký hotkey");
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Hủy đăng ký hotkey
            _hotkeyManager.Unregister();

            if (_trayIconManager != null)
            {
                {
                    _trayIconManager.Dispose();
                }
            }

            // Dừng theo dõi trạng thái pin
            _batteryPowerService.Stop();
        }

        private async void ShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            await _shutdownService.ShowShutdownConfirm();
        }
    }
}