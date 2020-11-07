using System;
using NLog;

namespace lib
{
    public class Class1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Class1()
        {
            
            logger.Info("Logger Start");
        }

        public void echo()
        {
            Console.Out.WriteLine("テスト");
            logger.Info("Console Test");
            logger.Debug("Debug Test");
        }
    }
}
