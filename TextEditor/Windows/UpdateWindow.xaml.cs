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

namespace TextEditor.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Wpf.Ui.Controls.UiWindow
    {
        public void SetStatusBar()
        {
            CheckingForUpdatesStatusBar.IsOpen= false;
            UpToDabeStatusBar.IsOpen= false;
            UpdateAvailableStatusBar.IsOpen= false;
            ServerUnavailableStatusBar.IsOpen= false; // i will leave it on this for now, while i get some sort of updates working
        }
        public UpdateWindow()
        {
            InitializeComponent();
            Globals.TS.SetSystemAccent();

            InstalledVerBlock.Text = Version.VersionString;
            SetStatusBar();
            string GitVersion = Helper.GetLatestVersionStringFromGitHub();
            LatestVersionBlock.Text = GitVersion;

            if (GitVersion == Version.VersionString)
            {
                // we will just assume its the latest version
                UpToDabeStatusBar.IsOpen = true;
            }
            else if (GitVersion != Version.VersionString)
            {
                UpdateAvailableStatusBar.IsOpen = true;
            }
        }
    }
}
