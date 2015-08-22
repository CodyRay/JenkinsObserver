using Contracts;
using Data;
using Gat.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ContextMenu = System.Windows.Forms.ContextMenu;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Forms.MenuItem;
using MessageBox = System.Windows.MessageBox;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace JenkinsObserver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon _notifyIcon;
        public string AppVersion = typeof(App).Assembly.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public ObserverSettings Settings { get; set; }
        public SettingsStorage Data { get; set; }
        public ObserverPoller Poller { get; set; }
        public TaskAsService PollerService { get; set; }

        public App()
        {
            Data = new SettingsStorage();
            Settings = Data.Settings; //This is here to cause entityframework to load a little faster...
            Poller = new ObserverPoller();
            PollerService = TaskAsService.Create(token => Poller.Run(token));
        }

        private void AppStart(object sender, StartupEventArgs e)
        {
            Uri uri;
            if (ApplicationUpdate.IsAvailable(new Uri("https://api.github.com/repos/haroldhues/JenkinsObserver/releases"), AppVersion, out uri))
            {
                if (
                    MessageBox.Show("Updates are available, would you like to upgrade?", "Update Available",
                        MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    Process.Start(uri.ToString()); //Open in browser
                    Shutdown(); //Kill Jenkins Observer
                }
            }

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
            var image = JenkinsObserver.Properties.Resources.appIcon.ToImageSource();
            var repoUrl = new Uri("https://github.com/haroldhues/JenkinsObserver/");
            var about = new About()
            {
                AdditionalNotes = "Jenkins Obsever is Licensed Under the MIT License. Application Icon: 'Binoculars' by Mourad Mokrane from the Noun Project, licensed under Creative Commons Attribution 3.0 License",
                ApplicationLogo = image,
                PublisherLogo = image,
                Hyperlink = repoUrl,
                Publisher = "Cody Ray Hoeft",
                HyperlinkText = repoUrl.ToString(),
                IsSemanticVersioning = false,
                Version = AppVersion
            };
            about.Show();
        }

        private void OpenConsole()
        {
            var console = new Console(this);
            console.Show();
        }

        public void JobChanged(ObserverPoller poller, ObserverServer server, ObserverJob job, ChangeType changeType)
        {
            var settings = Data.Settings;
            var enabled = (server?.Enabled ?? true) && (job?.Enabled ?? true);
            switch (changeType)
            {
                case ChangeType.BuildCompleted:
                    if (!(enabled && settings.EnableNotifications) || settings.AlertOnChangesOnly)
                        break;
                    _notifyIcon.ShowBalloonTip(2000, "Build Complete", $"Job: {job?.DisplayName} is no longer building", ToolTipIcon.Warning);
                    break;
                case ChangeType.BuildStarted:
                    if (!(enabled && settings.EnableNotifications) || settings.AlertOnChangesOnly)
                        break;
                    _notifyIcon.ShowBalloonTip(2000, "Build Started", $"Job: {job?.DisplayName} is now building", ToolTipIcon.Warning);
                    break;
                case ChangeType.BuildStatusChange:
                    if (!(enabled && settings.EnableNotifications))
                        break;
                    _notifyIcon.ShowBalloonTip(2000, "Status Change", $"Job: {job?.DisplayName} is now {job?.Status}", ToolTipIcon.Warning);
                    break;
                case ChangeType.MissingJob:
                    _notifyIcon.ShowBalloonTip(2000, "Job No Longer Exists", $"Job: {job?.DisplayName} no longer exists", ToolTipIcon.Warning);
                    break;
                case ChangeType.NewJobFound:
                    _notifyIcon.ShowBalloonTip(2000, "New Job", $"Job: {job?.DisplayName} has been found", ToolTipIcon.Warning);
                    break;
                case ChangeType.ErrorPollingServer:
                    _notifyIcon.ShowBalloonTip(10000, "Error Polling Server", $"Server: '{server?.Name}' at '{server?.Url}' could not be polled", ToolTipIcon.Error);
                    break;
                case ChangeType.ErrorPollingJob:
                    _notifyIcon.ShowBalloonTip(10000, "Error Polling Job", $"Job: '{job?.DisplayName ?? job?.Name}' in '{server?.Name}' could not be polled", ToolTipIcon.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(changeType), changeType, null);
            }
            if (enabled && settings.EnableSounds)
            {
                switch (changeType)
                {
                    case ChangeType.BuildCompleted:
                    case ChangeType.BuildStarted:
                        SystemSounds.Beep.Play();
                        break;
                    case ChangeType.BuildStatusChange:
                        SystemSounds.Hand.Play();
                        break;
                    case ChangeType.MissingJob:
                    case ChangeType.ErrorPollingServer:
                    case ChangeType.ErrorPollingJob:
                        SystemSounds.Asterisk.Play();
                        break;
                }

            }
        }

        public MainWindow SettingsWindow;

        public async void OpenSettings(object sender = null, EventArgs e = null)
        {
            if (SettingsWindow != null)
            {
                SettingsWindow.Focus();
                return;
            }

            var kc = new KonamiCodeStateMachine();
            SettingsWindow = new MainWindow(Data, Poller);
            SettingsWindow.KeyDown += (o, args) => kc.KeyPressed(args.Key);

            if (PollerService.IsRunning)
                await PollerService.Stop();

            kc.KonamiCodeEntered += o =>
            {
                SettingsWindow.Close();
                OpenConsole();
            };

            try
            {
                SettingsWindow.ShowDialog(); //TODO: Win32Exception The Operation Completed Successfully
            }
            catch (Win32Exception) { }

            PollerService.Start();
            _notifyIcon.ShowBalloonTip(2000, "Jenkins Observer", "Polling in Background", ToolTipIcon.Info);

            SettingsWindow = null;
        }

        private async void AppStop(object sender, EventArgs e)
        {
            SettingsWindow?.Close();

            if (PollerService.IsRunning)
                await PollerService.Stop();

            Shutdown();
        }
    }
}