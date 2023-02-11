using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TextEditor.Windows
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Wpf.Ui.Controls.UiWindow
    {
        public string GitVersion = string.Empty;
        public void SetStatusBar()
        {
            CheckingForUpdatesStatusBar.IsOpen= false;
            UpToDabeStatusBar.IsOpen= false;
            UpdateAvailableStatusBar.IsOpen= false;
            ServerUnavailableStatusBar.IsOpen= false;
        }
        public UpdateWindow()
        {
            InitializeComponent();
            //Thread.Sleep(100);
            Globals.TS.SetSystemAccent();

            InstalledVerBlock.Text = Version.VersionString;
            SetStatusBar();

            CheckingForUpdatesStatusBar.IsOpen = true;
            Thread.Sleep(90);
            GitVersion = Helper.GetLatestVersionStringFromGitHub();

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
