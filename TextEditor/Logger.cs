using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public class Logger
    {
        public static void Log(string message)
        {
            Globals.GlobalLog.Add(message);
        }
    }
}
