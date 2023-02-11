using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor.Pages
{
    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Wpf.Ui.Controls.UiPage
    {
        public AboutPage()
        {
            InitializeComponent();

            //BuildDateBlock.Text = "Build Time: " + Assembly.GetExecutingAssembly().GetLinkerTime().ToString(); // this caused the app to crash, will fix in a later release
            VersionBlock.Text = "Version: " + Version.VersionString;
        }
    }
}
