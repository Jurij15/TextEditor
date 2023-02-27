using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace TextEditor.Debugger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.UiWindow
    {
        bool bShouldSpamOpen = false;
        int AmountOfTimeOpen = 0;
        private readonly IThemeService _themeService;

        public MainWindow()
        {
            ThemeService ts = new ThemeService();
            InitializeComponent();

            Wpf.Ui.Appearance.Accent.ApplySystemAccent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_tick;
            timer.Start();
        }

        private void SpamOpen_Checked(object sender, RoutedEventArgs e)
        {
            bShouldSpamOpen=true;
        }

        private void SpamOpen_Unchecked(object sender, RoutedEventArgs e)
        {
            bShouldSpamOpen=false;
        }

        void timer_tick(object sender, EventArgs e)
        {
            if (bShouldSpamOpen)
            {
                TextEditor.MainWindow main = new TextEditor.MainWindow();
                main.Show();
                AmountOfTimeOpen++;
            }
            OpenedAmount.Text = AmountOfTimeOpen.ToString();
        }

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void CloseAlllBTn_Click(object sender, RoutedEventArgs e)
        {
            var Processes = Process.GetProcesses();
            foreach (var process in Processes)
            {
                if (process.ProcessName == "TextEdit")
                {
                    process.Kill();
                }
            }
        }
    }
}
