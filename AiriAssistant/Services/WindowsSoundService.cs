using Microsoft.Win32;

namespace AiriAssistant.Services
{
    public class WindowsSoundService
    {
        private const string BasePath =
            @"AppEvents\Schemes\Apps\.Default";

        private string? _deviceConnectBackup;
        private string? _deviceDisconnectBackup;
        private string? _lowBatteryAlarmBackup;

        public void DisableSound()
        {
            BackupSoundEvent("DeviceConnect");
            BackupSoundEvent("DeviceDisconnect");
            BackupSoundEvent("LowBatteryAlarm");
        }

        private void BackupSoundEvent(string eventName)
        {
            string path = $"{BasePath}\\{eventName}\\.Current";

            using RegistryKey? key =
                Registry.CurrentUser.OpenSubKey(path, true);

            if (key == null)
                return;

            string? currentValue = key.GetValue("")?.ToString();

            if (eventName == "DeviceConnect")
                _deviceConnectBackup = currentValue;

            if (eventName == "DeviceDisconnect")
                _deviceDisconnectBackup = currentValue;

            if(eventName == "LowBatteryAlarm")
                _lowBatteryAlarmBackup = currentValue;

            key.SetValue("", "");
        }

        public void RestoreSound()
        {
            RestoreEvent("DeviceConnect", _deviceConnectBackup);
            RestoreEvent("DeviceDisconnect", _deviceDisconnectBackup);
            RestoreEvent("LowBatteryAlarm", _lowBatteryAlarmBackup);
        }

        private void RestoreEvent(
    string eventName,
    string? value
)
        {
            if (value == null)
                return;

            string path = $"{BasePath}\\{eventName}\\.Current";

            using RegistryKey? key =
                Registry.CurrentUser.OpenSubKey(path, true);

            if (key == null)
                return;

            key.SetValue("", value);
        }
    }
}
