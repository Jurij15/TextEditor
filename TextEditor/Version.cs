using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public static class Version
    {
        public enum VersionTypes : int
        {
            Debug,
            Testing,
            Unstable,
            Release
        }

        public static string GetVersionTypeStringFromEnum(VersionTypes VersionType)
        {
            switch (VersionType)
            {
                case VersionTypes.Debug:
                    return "Debug";
                    break;
                case VersionTypes.Testing:
                    return "Testing";
                    break; 
                case VersionTypes.Unstable:
                    return "Unstable";
                    break;
                case VersionTypes.Release:
                    return "Release";
                    break;
            }

            return null;
        }

        public static double VersionNumber = 0.5;
        public static string versionType = GetVersionTypeStringFromEnum(VersionTypes.Debug);

        public static string VersionString = VersionNumber.ToString() + "-" + versionType.ToString();

        public static string AboutString = "A simple notepad project" + Environment.NewLine + Environment.NewLine + "Made with <3 by Jurij15";
    }
}
