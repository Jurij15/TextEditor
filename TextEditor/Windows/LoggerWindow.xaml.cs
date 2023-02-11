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
using System.Windows.Threading;
using TextEditor.Dialogs;

namespace TextEditor.Windows
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        public LoggerWindow()
        {
            InitializeComponent();
            Wpf.Ui.Appearance.Theme.ApplyDarkThemeToWindow(this);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_tick;
            timer.Start();
        }

        void timer_tick(object sender, EventArgs e)
        {
            LogBox.Items.Clear();
            foreach (var item in Globals.GlobalLog)
            {
                LogBox.Items.Add(item);
            }
        }
    }
}
