using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.UnityPlugins
{
    public enum LogLevel
    {
        None,
        Info,
        Warning,
        Error,
        Fatal
    }

    public class DebugLog
    {
        public static LogLevel CurrentLogLevel = LogLevel.None;
        
        public static void Log(LogLevel level, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                // non-fatal condition, ignore it
                return;    
            }

            // sanity check check the log level
            if (level < LogLevel.Info || level > LogLevel.Fatal)
            {
                return;     
            }

            switch (level)
            {
                case LogLevel.Info:
                {
                    Debug.WriteLine("INFO: " + message);
                    break;
                }
                case LogLevel.Warning:
                {
                    Debug.WriteLine("WARNING: " + message);
                    break;
                }
                case LogLevel.Error:
                {
                    Debug.WriteLine("ERROR: " + message);
                    break;
                }
                case LogLevel.Fatal:
                {
                    Debug.WriteLine("FATAL: " + message);
                    break;
                }
            }
        }
    }
}
