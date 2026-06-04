using System.Threading.Tasks;

namespace AiriAssistant.Services
{
    public class ShutdownService
    {
        private readonly SoundService _soundService;
        private readonly PowerService _powerService;

        public ShutdownService(SoundService soundService, PowerService powerService)
        {
            _soundService = soundService;
            _powerService = powerService;
        }

        public async Task ShowShutdownConfirm()
        {
            _soundService.Play("Asset\\Sounds\\WindowsPower\\shutdown_question.wav");

            ShutdownConfirmWindow confirmWindow = new();
            confirmWindow.ShowDialog();

            if (confirmWindow.IsConfirmed)
            {
                _soundService.Play("Asset\\Sounds\\WindowsPower\\shutdown_confirm.wav");

                await Task.Delay(2700);

                System.Windows.MessageBox.Show("ここでシャットダウンします。");
                _powerService.Shutdown();
            }
        }
    }
}