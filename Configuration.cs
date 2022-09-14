using System.Collections.Generic;

namespace Mettarin.Android
{
    public class Configuration
    {
        public List<string> ModulePrefixes { get; set; }

        public LoggerConfiguration Logger { get; set; }
    }

    public class LoggerConfiguration
    {
        public bool DebugMode { get; set; }

        public bool SaveLocalizedExceptions { get; set; }
    }
}
