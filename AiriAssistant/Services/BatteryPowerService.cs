using Microsoft.Win32;

namespace AiriAssistant.Services
{
    public class BatteryPowerService
    {
        private readonly SoundService _soundService;
        private PowerLineStatus _lastPowerStatus;

        public BatteryPowerService(SoundService soundService)
        {
            _soundService = soundService;
            _lastPowerStatus = SystemInformation.PowerStatus.PowerLineStatus;
        }

        public void Start()
        {
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
        }

        public void Stop()
        {
            SystemEvents.PowerModeChanged -= OnPowerModeChanged;
        }

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode != PowerModes.StatusChange) return;

            PowerLineStatus currentStatus = SystemInformation.PowerStatus.PowerLineStatus;

            if (currentStatus == _lastPowerStatus) return;

            _lastPowerStatus = currentStatus;

            if (currentStatus == PowerLineStatus.Online)
            {
                _soundService.Play("Asset\\Sounds\\System\\charger_connected.wav");
            }
            else if (currentStatus == PowerLineStatus.Offline)
            {
                _soundService.Play("Asset\\Sounds\\System\\charger_disconnected.wav");
            }
        }
    }
}
