using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static int TimeSinceLastTabsLogDump = 0;// we will dump tabs state into log every 5 seconds
    }
}
