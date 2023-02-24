using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TextEditor.Functions;

namespace TextEditor.Windows
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Wpf.Ui.Controls.UiWindow
    {
        int counter  = 0;
        DispatcherTimer Welcometimer = new DispatcherTimer();
        public void IncreaseProgress()
        {
            int counter = 0;
            while(counter < 1000000)
            {
                //WelcomeProcessRing.HasAnimatedProperties = true;
                counter++;
            }
            MessageBox.Show("done");
        }

        async Task PutTaskDelayWelcomeFirstTime()
        {
            await Task.Delay(2500);
        }

        async Task PutTaskDelayWelcomeBack()
        {
            await Task.Delay(800);
        }

        public WelcomeWindow()
        {
            InitializeComponent();
            Settings.GetSettings();
            //Wpf.Ui.Appearance.Watcher.Watch(this);
            OnStartup();
            WelcomeProcessRing.IsIndeterminate = true;
        }

        private async void OnStartup()
        {
            if (Globals.bShouldShowWelcomeWindow)
            {
                await PutTaskDelayWelcomeFirstTime();
                Globals.bShouldShowWelcomeWindow = false;
                //RestartApp();
                this.Hide();
                BetterMainWindow betterMainWindow = new BetterMainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
            else if (Settings.SettingsValues.bShouldShowWelcomeBackWindow)
            {
                PreparingAppBlock.Text = "  Loading The Application";
                await PutTaskDelayWelcomeBack();
                //Settings.SettingsValues.bShouldShowWelcomeBackWindow = false;
                this.Hide();
                BetterMainWindow betterMainWindow = new BetterMainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
            else
            {
                this.Hide();
                BetterMainWindow betterMainWindow = new BetterMainWindow();
                betterMainWindow.Show();
                betterMainWindow.ShowActivated = true;
                betterMainWindow.ShowInTaskbar = true;
            }
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.Close();
        }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //IncreaseProgress();
        }
    }
}
