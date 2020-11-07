using System;
using NLog;
using System.Net;
using System.Net.NetworkInformation;
using Amazon.DynamoDBv2;
namespace lib
{

    public class Class1 : AbstractClass1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public Class1()
        {
            logger.Info("resouce opening...");
            Ip = "localhost";
            Port = 8000;
            EndpointUrl = string.Format("http://{0}:{1}", Ip, Port);
            logger.Info("resouce opened");
        }

        ~Class1()
        {
            logger.Info("resource closing...");
            destructor();
            logger.Info("resource closed.");
        }
        public override void destructor()
        {
            logger.Info("destructor start");

            logger.Info("destructor finish");
        }
    }
}
