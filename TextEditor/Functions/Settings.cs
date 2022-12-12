using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Functions
{
    public class Settings
    {
        public static string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");

        public static string RootAppDataDir = LocalAppData + "/TextEditor";

        public static string AppDataSavesDir = RootAppDataDir + "/Saves";

        public static string AppDataThemeConfigFile = AppDataSavesDir + "theme";

        public static void GetSettings()
        {

        }
    }
}
