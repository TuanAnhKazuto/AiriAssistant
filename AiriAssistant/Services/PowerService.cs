using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AiriAssistant.Services
{
    public class PowerService
    {
        public void Shutdown()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "shutdown.exe",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(psi);
        }

        public void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }

        public void LockScreen()
        {
            LockWorkStation();
        }

        public void Sleep()
        {
            SetSuspendState(false, true, true);
        }

        [DllImport("user32.dll")]
        private static extern bool LockWorkStation();

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern bool SetSuspendState(
            bool hibernate,
            bool forceCritical,
            bool disableWakeEvent
        );
    }
}