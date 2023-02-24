using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Functions;

namespace TextEditor
{
    public static class Helper
    {
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
        public static string GetLatestVersionStringFromGitHub()
        {
            string ReturnValue = string.Empty;
            if (File.Exists(Settings.AppDataTempDir + "TempLatestVersionFile"))
            {
                File.Delete(Settings.AppDataTempDir + "TempLatestVersionFile");
            }
            WebClient wc = new WebClient();
            wc.DownloadFile(@"https://raw.githubusercontent.com/Jurij15/TextEditor/master/TextEditor/Releases/LatestVersion.txt", Settings.AppDataTempDir + "TempLatestVersionFile");

            ReturnValue = File.ReadAllText(Settings.AppDataTempDir + "TempLatestVersionFile");

            return ReturnValue; ;
        }

        public static void RestartApp()
        {
            /* this doesnt work idk why
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 1 & START \"\" \"" + Assembly.GetEntryAssembly().Location + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Process.GetCurrentProcess().Kill();
            */

            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        public static Process GetProcessByMainWindoName(string MainWindowName)
        {
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.MainWindowTitle == MainWindowName)
                {
                    return process;
                }
            }

            return null;
        }
    }
}
