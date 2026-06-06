using Microsoft.Win32;

namespace AiriAssistant.Services
{
    public class WindowsSoundService
    {
        private const string BasePath =
            @"AppEvents\Schemes\Apps\.Default";

        public void DisableSound()
        {
            DisableSoundEvent("DeviceConnect");
            DisableSoundEvent("DeviceDisconnect");
        }

        private void DisableSoundEvent(string eventName)
        {
            string path = $"{BasePath}\\{eventName}\\.Current";

            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(path, writable: true);

            if (key == null) return;

            key.SetValue("", "");
        }
    }
}
