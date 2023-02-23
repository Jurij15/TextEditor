using System;
using System.Collections.Generic;
using System.Linq;
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
        public WelcomeWindow()
        {
            InitializeComponent();
            //Thread increaseProgressThread = new Thread( new ThreadStart(IncreaseProgress));
            Welcometimer.Interval = TimeSpan.FromSeconds(1);
            Welcometimer.Tick += timer_tick2;
            Welcometimer.Start();
        }

        void timer_tick2(object sender, EventArgs e)
        {
            //we will just show it for 5 secs
            if (counter >= 3)
            {
                counter++;
            }
            else
            {
                Welcometimer.Stop();
                this.Close();
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
