using System;
using System.Drawing;
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
            _trayIcon = new NotifyIcon();

            _trayIcon.Icon = SystemIcons.Application;
            _trayIcon.Text = "Airi Assistant";
            _trayIcon.Visible = true;

            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem shutdownItem = new ToolStripMenuItem("Shutdown");
            shutdownItem.Click += (s, e) => ShutdownClicked?.Invoke();

            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => ExitClicked?.Invoke();

            menu.Items.Add(shutdownItem);
            menu.Items.Add(exitItem);

            _trayIcon.ContextMenuStrip = menu;
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