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
            public static Wpf.Ui.Appearance.WindowCornerPreference CornerPreference = Wpf.Ui.Appearance.WindowCornerPreference.Round;
            public static bool StatusBarVisibility = true;
        }
        public static string LocalAppData = Environment.GetEnvironmentVariable("LocalAppData");

        public static string RootAppDataDir = LocalAppData + "/TextEditor";

        public static string AppDataSavesDir = RootAppDataDir + "/Saves/";

        public static string AppDataTempDir = RootAppDataDir + "/Temp/";

        public static string AppDataThemeConfigFile = AppDataSavesDir + "theme";
        public static string AppDataToolBarVisibilityConfigFile = AppDataSavesDir + "ToolbarVisibility";
        public static string AppDataCornerPreferenceConfigFile = AppDataSavesDir + "CornerPref";
        public static string AppDataStatusBarVisibilityConfigFile = AppDataSavesDir + "StatBarVisibility";


        public static void CreateSettings() //create the settings folder and configs
        {
            Globals.bShouldShowWelcomeWindow = true;
            if (Directory.Exists(RootAppDataDir))
            {
                Directory.Delete(RootAppDataDir, true); // this will delete anything already in there
            }

            Directory.CreateDirectory(AppDataSavesDir);
            Directory.CreateDirectory(AppDataTempDir);

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

            using (StreamWriter sw = File.CreateText(AppDataCornerPreferenceConfigFile))
            {
                sw.WriteLine(SettingsValues.CornerPreference.ToString());
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(AppDataStatusBarVisibilityConfigFile))
            {
                sw.WriteLine(SettingsValues.StatusBarVisibility.ToString());
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
                SettingsValues.StatusBarVisibility = Convert.ToBoolean(File.ReadAllText(AppDataStatusBarVisibilityConfigFile));
                Enum.TryParse<Wpf.Ui.Appearance.WindowCornerPreference>(File.ReadAllText(AppDataCornerPreferenceConfigFile), out SettingsValues.CornerPreference);
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

        public static void ResetSettings()
        {
            Directory.Delete(RootAppDataDir, true);
            CreateSettings();
        }

        public static void ChangeWindowCornerPreferenceSetting(Wpf.Ui.Appearance.WindowCornerPreference CornerPref)
        {
            if (File.Exists(AppDataCornerPreferenceConfigFile))
            {
                File.Delete(AppDataCornerPreferenceConfigFile);
            }
            using (StreamWriter sw = File.CreateText(AppDataCornerPreferenceConfigFile))
            {
                sw.WriteLine(CornerPref.ToString());
                sw.Close();
            }
        }

        public static void ChangeStatusBarVisuiblitySetting(bool NewValue)
        {
            if (File.Exists(AppDataStatusBarVisibilityConfigFile))
            {
                File.Delete(AppDataStatusBarVisibilityConfigFile);
            }
            using (StreamWriter sw = new StreamWriter(AppDataStatusBarVisibilityConfigFile))
            {
                sw.WriteLine(NewValue.ToString());
                sw.Close();
            }
        }

        public static void ChangeToolBarVisuiblitySetting(bool NewValue)
        {
            if (File.Exists(AppDataToolBarVisibilityConfigFile))
            {
                File.Delete(AppDataToolBarVisibilityConfigFile);
            }
            using (StreamWriter sw = new StreamWriter(AppDataToolBarVisibilityConfigFile))
            {
                sw.WriteLine(NewValue.ToString());
                sw.Close();
            }
        }
    }
}
