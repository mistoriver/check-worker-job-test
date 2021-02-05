using log4net;
using log4net.Config;
using System;

namespace CheckWatcher
{
    public static class ServiceLogger
    {
        private static readonly ILog log = LogManager.GetLogger("CheckLogger");


        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
