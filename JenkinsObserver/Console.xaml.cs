using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Contracts;
using Data;

namespace JenkinsObserver
{
    /// <summary>
    /// Interaction logic for Console.xaml
    /// </summary>
    public partial class Console : Window
    {
        protected App RunningApp;

        public Console()
        {
            InitializeComponent();
        }

        public Console(App app)
        {
            InitializeComponent();
            RunningApp = app;
        }

        public void Console_OnLoaded(object sender, RoutedEventArgs e)
        {
            comboBox.ItemsSource = Enum.GetValues(typeof(ConsoleCommands)).Cast<ConsoleCommands>();
        }

        private void Console_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            var command = comboBox.SelectedItem as ConsoleCommands?;
            if(command.HasValue)
                RunCommand(command.Value);
            else if (comboBox.SelectedItem is string)
            {
                ConsoleCommands parsedCommand;
                if (Enum.TryParse(comboBox.SelectedItem as string, out parsedCommand))
                {
                    RunCommand(parsedCommand);
                }
            }
        }

        private async void RunCommand(ConsoleCommands command)
        {
            try
            {
                output.Text = "";
                switch (command)
                {
                    case ConsoleCommands.DeleteDatabase:
                        await RunningApp.PollerService.Stop();
                        RunningApp.Data.DeleteDatabase();
                        break;

                    case ConsoleCommands.GetSettingsJson:
                        await RunningApp.PollerService.Stop();
                        output.Text = RunningApp.Data.SettingsAsJson;
                        break;

                    case ConsoleCommands.ClearDatabase:
                        await RunningApp.PollerService.Stop();
                        RunningApp.Data.ClearData();
                        break;

                    case ConsoleCommands.Settings:
                        RunningApp.OpenSettings();
                        break;

                    case ConsoleCommands.PollerRunning:
                        output.Text = RunningApp.PollerService.IsRunning.ToString();
                        break;

                    case ConsoleCommands.PollerStart:
                        RunningApp.PollerService.Start();
                        break;

                    case ConsoleCommands.PollerStop:
                        await RunningApp.PollerService.Stop();
                        break;

                    default:
                        throw new InvalidOperationException("Command Not Supported");
                }
            }
            catch (Exception ex)
            {
                output.Text = ex.ToString();
            }
        }
    }
}
