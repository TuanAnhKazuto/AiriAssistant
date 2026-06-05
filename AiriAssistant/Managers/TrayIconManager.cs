using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace AiriAssistant.Managers
{
    public class TrayIconManager
    {
        private NotifyIcon _trayIcon;

        public event Action ShutdownClicked;
        public event Action ExitClicked;

        public void Initialize()
        {
            _trayIcon = new();

            _trayIcon.Icon = new(TrayIconImage("Asset\\Icon\\icon.ico"));
            _trayIcon.Text = "Airi Assistant";
            _trayIcon.Visible = true;

            ContextMenuStrip menu = new();

            ToolStripMenuItem shutdownItem = new("Shutdown");
            shutdownItem.Click += (s, e) => ShutdownClicked?.Invoke();

            ToolStripMenuItem exitItem = new("Exit");
            exitItem.Click += (s, e) => ExitClicked?.Invoke();

            menu.Items.Add(shutdownItem);
            menu.Items.Add(exitItem);

            _trayIcon.ContextMenuStrip = menu;
        }

        private string TrayIconImage(string path)
        {
            string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            return fullPath;
        }

        public void Dispose()
        {
            if (_trayIcon == null) return;

            _trayIcon.Visible = false;
            _trayIcon.Dispose();
            _trayIcon = null;
        }
    }
}