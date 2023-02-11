using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Functions
{
    public class Settings
    {
        public static class SettingsValues
        {
            public static string Theme = "DARK"; //default value (will be overwritten with the saved one) ((MAKE SURE ITS IN ALL CAPS!!!))
            public static bool ToolbarVisibility = true; //default value (will be overwritten with the saved one)
        }
        public static string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");

        public static string RootAppDataDir = LocalAppData + "/TextEditor";

        public static string AppDataSavesDir = RootAppDataDir + "/Saves/";

        public static string AppDataThemeConfigFile = AppDataSavesDir + "theme";
        public static string AppDataToolBarVisibilityConfigFile = AppDataSavesDir + "ToolbarVisibility";

        public static void CreateSettings() //create the settings folder and configs
        {
            if (Directory.Exists(RootAppDataDir))
            {
                Directory.Delete(RootAppDataDir, true); // this will delete anything already in there
            }

            Directory.CreateDirectory(AppDataSavesDir);
            Directory.CreateDirectory(AppDataSavesDir);

            //now create the files
            using (StreamWriter sw = File.CreateText(AppDataThemeConfigFile))
            {
                sw.WriteLine(SettingsValues.Theme);
                sw.Close();
            }

            using (StreamWriter sw = File.CreateText(AppDataToolBarVisibilityConfigFile))
            {
                sw.WriteLine(SettingsValues.ToolbarVisibility.ToString());
                sw.Close();
            }
        }

        public static void GetSettings()
        {
            if (!Directory.Exists(AppDataSavesDir)) // if it doesnt exist, create it
            {
                CreateSettings();
            }
            else
            {
                SettingsValues.Theme = File.ReadAllText(AppDataThemeConfigFile);
                SettingsValues.ToolbarVisibility = Convert.ToBoolean(File.ReadAllText(AppDataToolBarVisibilityConfigFile));
            }
        }

        public static void SwitchThemeSetting() //there is only dark and light theme
        {
            File.Delete(AppDataThemeConfigFile);
            if (Globals.TS.GetTheme() == Wpf.Ui.Appearance.ThemeType.Dark)
            {
                using (StreamWriter sw = File.CreateText(AppDataThemeConfigFile))
                {
                    sw.WriteLine("DARK");
                    sw.Close();
                }

                SettingsValues.Theme = "DARK";
            }
            else
            {
                using (StreamWriter sw = File.CreateText(AppDataThemeConfigFile))
                {
                    sw.WriteLine("LIGHT");
                    sw.Close();
                }

                SettingsValues.Theme = "LIGHT";
            }
        }
    }
}
