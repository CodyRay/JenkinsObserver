using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using Autofac.Features.OwnedInstances;
using Contracts;
using Data;

namespace JenkinsObserver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SettingsStorage Data { get; set; }
        public ObserverPoller Poller { get; set; }
        public ObserverSettings Settings
        {
            get
            {
                return DataContext as ObserverSettings;
            }
            set
            {
                Data.Settings = value;
                DataContext = value;
            }
        }

        public MainWindow(SettingsStorage data, ObserverPoller poller)
        {
            Data = data;
            Poller = poller;
            InitializeComponent();
        }

        private void AddServer(object sender, RoutedEventArgs e)
        {
            Settings.Servers.Add(new ObserverServer
            {
                Name = "New Server"
            });
        }

        //I think that it is dumb that I have to handle this, but the wheel gets eaten by a child it needs to be moved manually
        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(60.0);
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta / 3);
        }

        private async void PollNow_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null)
                return;
            var server = fe.DataContext as ObserverServer;
            if (server == null)
                return;
            try
            {
                fe.IsEnabled = false;
                await Poller.PollServer(server, CancellationToken.None);
            }
            finally
            {
                fe.IsEnabled = true;
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Close(); //Don't ignore this window buddy!
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Data.Settings = Settings;
            Deactivated -= Window_Deactivated; //Prevent an exception from trying to close the window twice
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Settings = Data.Settings;
        }
    }
}
