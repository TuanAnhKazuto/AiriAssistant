using System;
using System.Windows.Forms;
using System.Windows.Threading;

namespace AiriAssistant.Services
{
    public class BatteryWarningService
    {
        private readonly SoundService _soundService;
        private readonly DispatcherTimer _timer;

        private bool _hasPlayedLowWarning;
        private bool _hasPlayedCriticalWarning;

        public BatteryWarningService(SoundService soundService)
        {
            _soundService = soundService;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            CheckBattery();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            CheckBattery();
        }

        private void CheckBattery()
        {
            PowerStatus powerStatus = SystemInformation.PowerStatus;

            int batteryPercent = (int)(powerStatus.BatteryLifePercent * 100);
            bool isCharging = powerStatus.PowerLineStatus == PowerLineStatus.Online;

            if (isCharging)
            {
                _hasPlayedLowWarning = false;
                _hasPlayedCriticalWarning = false;
                return;
            }

            if (batteryPercent <= 10 && !_hasPlayedCriticalWarning)
            {
                _soundService.Play("Asset\\Sounds\\Battery\\battery_critical.wav");
                _hasPlayedCriticalWarning = true;
                _hasPlayedLowWarning = true;
                return;
            }

            if (batteryPercent <= 20 && !_hasPlayedLowWarning)
            {
                _soundService.Play("Asset\\Sounds\\Battery\\battery_low.wav");
                _hasPlayedLowWarning = true;
            }
        }
    }
}