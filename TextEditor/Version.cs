using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public static class Version
    {
        //just for context, dont actually use this
        public enum VersionTypes : int
        {
            Debug,
            Testing,
            Unstable,
            Release
        }
        public static double VersionNumber = 0.1;
        public static string versionType = "Debug";

        public static string VersionString = VersionNumber.ToString()+"-"+versionType;

        public static string AboutString = "A simple notepad project" + Environment.NewLine + Environment.NewLine + "Made with <3 by Jurij15";
    }
}
