using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AiriAssistant.Services;

namespace AiriAssistant
{
    public partial class App : System.Windows.Application
    {
        private readonly SoundService _soundService = new();

        private static Mutex _mutex;

        public bool IsRunning { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            const string appName = "AiriAssistant_SingleInstance";

            _mutex = new Mutex(true, appName, out bool isNewInstance);

            if (!isNewInstance)
            {
                IsRunning = true;
                _soundService.Play("Asset\\Sounds\\Airi_Infor\\airi_is_here.wav");

                System.Windows.MessageBox.Show(
                    "アイリ はすでに起動しています。",
                    "Airi Assistant"
                );

                Shutdown();
                return;
            }
            else
            {
                IsRunning = false;
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();

            base.OnExit(e);
        }
    }
}