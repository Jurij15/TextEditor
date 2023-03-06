using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using TextEditor.Functions;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Services;

namespace TextEditor
{
    public class Globals
    {
        public static List<string> GlobalLog = new List<string>();
        public static ThemeService TS;
        internal static int RunningTimeInSeconds;
        internal static DateTime CurrentDateTime;
        internal static string AppTitle = "TextEditor";
        public static UiWindow MainWindow;
        public static object MainWindowObject;

        public static bool bShouldShowWelcomeWindow = false;

        public static bool IsHardwareAccelerationSupported()
        {
            if (Wpf.Ui.Hardware.HardwareAcceleration.IsSupported(Wpf.Ui.Hardware.RenderingTier.FullAcceleration)) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int TimeSinceLastTabsLogDump = 0;// we will dump tabs state into log every 5 seconds

        public static void ChangeTheme()
        {
            //Globals.TS.SetTheme(Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark ? Wpf.Ui.Appearance.ThemeType.Light : Wpf.Ui.Appearance.ThemeType.Dark);
            if (Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
            {
                Globals.TS.SetTheme(Wpf.Ui.Appearance.ThemeType.Light);
            }
            else
            {
                Globals.TS.SetTheme(Wpf.Ui.Appearance.ThemeType.Dark);
            }
            Settings.SwitchThemeSetting();
        }

        public static bool bIsWindows11()
        {
            OperatingSystem os = Environment.OSVersion;
            if (os.Version.Build >= 22000)
            {
                return true;
            }
            else { return false; }
        }
    }
}
