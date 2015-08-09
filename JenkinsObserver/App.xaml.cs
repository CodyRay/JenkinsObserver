using Contracts;
using Data;
using Gat.Controls;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace JenkinsObserver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _notifyIcon;
        protected ObserverSettings Settings { get; set; }
        protected SettingsStorage Data { get; set; }
        protected ObserverPoller Poller { get; set; }
        protected TaskAsService PollerService { get; set; }

        public App()
        {
            Data = new SettingsStorage();
            Settings = Data.Settings; //This is here to cause entityframework to load a little faster...
            Poller = new ObserverPoller();
            PollerService = TaskAsService.Create(token => Poller.Run(token));
        }

        private void AppStart(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            #region TrayIcon

            //Init tray icon
            _notifyIcon = new NotifyIcon
            {
                Text = "Jenkins Observer",
                Visible = true,
                ContextMenu = new ContextMenu(),
                Icon = JenkinsObserver.Properties.Resources.appIcon
            };
            //Init tray icon menu
            var menuSettings = new MenuItem
            {
                Text = "Settings",
                DefaultItem = true
            };
            menuSettings.Click += OpenSettings;
            _notifyIcon.ContextMenu.MenuItems.Add(menuSettings);
            var about = new MenuItem
            {
                Text = "About",
            };
            about.Click += OpenAbout;
            _notifyIcon.ContextMenu.MenuItems.Add(about);

            _notifyIcon.ContextMenu.MenuItems.Add("-");

            var menuExit = new MenuItem { Text = "Exit" };
            menuExit.Click += AppStop;
            _notifyIcon.ContextMenu.MenuItems.Add(menuExit);

            _notifyIcon.BalloonTipClicked += OpenSettings;
            _notifyIcon.DoubleClick += OpenSettings;
            _notifyIcon.MouseClick += (o, args) =>
            {
                if (args.Button == MouseButtons.Left)
                    OpenSettings();
            };

            #endregion TrayIcon

            Poller.JobChanged += JobChanged;

            OpenSettings();
        }

        private void OpenAbout(object sender = null, EventArgs e = null)
        {
            var version = typeof(App).Assembly.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var image = JenkinsObserver.Properties.Resources.appIcon.ToImageSource();
            var repoUrl = new Uri("https://github.com/haroldhues/JenkinsObserver/");
            var about = new About()
            {
                AdditionalNotes = "Jenkins Obsever is Licensed Under the MIT License. This application will also attempt to update itself on startup.",
                ApplicationLogo = image,
                PublisherLogo = image,
                Hyperlink = repoUrl,
                Publisher = "Cody Ray Hoeft",
                HyperlinkText = repoUrl.ToString(),
                IsSemanticVersioning = false,
                Version = version
            };
            about.Show();
        }

        private void JobChanged(ObserverPoller poller, ObserverJob job, ChangeType changeType)
        {
            _notifyIcon.ShowBalloonTip(2000, "JobChanged", $"Job: {job.DisplayName} is now {job.Status}", ToolTipIcon.Warning);
        }

        private MainWindow _settings;

        private async void OpenSettings(object sender = null, EventArgs e = null)
        {
            if (_settings != null)
            {
                _settings.Focus();
                return;
            }
            _settings = new MainWindow(Data, Poller);

            if (PollerService.IsRunning)
                await PollerService.Stop();

            _settings.ShowDialog(); //TODO: Win32Exception The Operation Completed Successfully

            PollerService.Start();
            _notifyIcon.ShowBalloonTip(2000, "Jenkins Observer", "Polling in Background", ToolTipIcon.Info);

            _settings = null;
        }

        private async void AppStop(object sender, EventArgs e)
        {
            _settings?.Close();

            if (PollerService.IsRunning)
                await PollerService.Stop();

            Shutdown();
        }
    }
}