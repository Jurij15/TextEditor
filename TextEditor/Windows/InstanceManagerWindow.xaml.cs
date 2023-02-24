using Microsoft.VisualBasic.Devices;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TextEditor.Windows
{
    /// <summary>
    /// Interaction logic for InstanceManagerWindow.xaml
    /// </summary>
    public partial class InstanceManagerWindow : Wpf.Ui.Controls.UiWindow
    {
        List<string> instances = new List<string>();
        void InitValues()
        {
            Process[] AlProcesses = Process.GetProcesses();
            foreach (Process instance in AlProcesses)
            {
                if (instance.ProcessName.Contains("TextEditor"))
                {
                    instances.Add(instance.ProcessName);
                }
                if (instance.ProcessName.Contains("InstanceManager"))
                {
                    continue;
                }
            }

            foreach (var item in instances)
            {
                InstancesBox.Items.Add(item);
            }
        }
        public InstanceManagerWindow()
        {
            InitializeComponent();
            DispatcherTimer RefreshTimer = new DispatcherTimer(); 
            RefreshTimer.Interval = TimeSpan.FromMilliseconds(500);
            RefreshTimer.Tick += timer_tick;
            RefreshTimer.Start();
            RefreshCard_Click(null, null);
        }

        void timer_tick(object sender, EventArgs e)
        {
            //InstancesBox.Items.Clear();
            //instances.Clear();
            //InitValues();
            CurrentlyOpenedInstancesCard.Content = "Currently opened instances: "+instances.Count;
        }

        private void RefreshCard_Click(object sender, RoutedEventArgs e)
        {
            InstancesBox.Items.Clear();
            instances.Clear();
            InitValues();
        }

        private void KillInstanceCard_Click(object sender, RoutedEventArgs e)
        {
            if (InstancesBox.SelectedItem == null)
            {
                return;
            }

            var SelectedItem = InstancesBox.SelectedItem;
            Helper.GetProcessByMainWindoName(SelectedItem.ToString()).Kill();
        }

        private void NewInstanceCard_Click(object sender, RoutedEventArgs e)
        {
            BetterMainWindow window = new BetterMainWindow();
            window.Show();
        }
    }
}
