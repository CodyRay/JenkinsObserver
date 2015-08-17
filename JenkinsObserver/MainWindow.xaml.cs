using Contracts;
using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public bool HoldOpen { get; set; }

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
                Name = "New Server",
                Url = @"http://example.com/",
                Enabled = true,
            });
        }

        //I think that it is dumb that I have to handle this, but the wheel gets eaten by a child it needs to be moved manually
        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(60.0);
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta / 3);
        }

        private readonly List<CancellationTokenSource> _tokenSources = new List<CancellationTokenSource>();
        private async void PollNow_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var server = (fe)?.DataContext as ObserverServer;
            if (server == null)
                return;
            if (!CheckUrl(server.Url))
                return;
            var tokenSource = new CancellationTokenSource();
            try
            {
                fe.IsEnabled = false;
                _tokenSources.Add(tokenSource);
                await Poller.PollServer(server, tokenSource.Token);
            }
            finally
            {
                _tokenSources.Remove(tokenSource);
                fe.IsEnabled = true;
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if(!HoldOpen)
                Close(); //Don't ignore this window buddy!
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if(Settings != null)
                Data.Settings = Settings;
            foreach(var tSource in _tokenSources)
                tSource?.Cancel();
            Deactivated -= Window_Deactivated; //Prevent an exception from trying to close the window twice
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Settings = Data.Settings;
            Icon = Properties.Resources.appIcon.ToImageSource();
        }

        private void Visit_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var server = (fe)?.DataContext as ObserverServer;
            if (server == null)
                return;
            if(!CheckUrl(server.Url))
                return;
            Process.Start(new Uri(server.Url).ToString());
        }

        private bool CheckUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"'{url}' is not a valid Url");
                return false;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var fe = sender as FrameworkElement;
            var server = (fe)?.DataContext as ObserverServer;
            if (server == null)
                return;
            Settings.Servers.Remove(server);
        }
    }
}