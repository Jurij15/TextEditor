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
        public static ThemeService TS;
        internal static int RunningTimeInSeconds;
        internal static DateTime CurrentDateTime;
        internal static string AppTitle = "TextEditor";
    }
}
