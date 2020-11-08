using System;
using NLog;
using System.Net;
using System.Net.NetworkInformation;
using Amazon.DynamoDBv2;
namespace lib
{

    public class Class1 : AbstractClass1<AmazonDynamoDBClient>,IClass1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public Class1()
        {
            Ip = "localhost";
            Port = 8000;
            EndpointUrl = string.Format("http://{0}:{1}", Ip, Port);

            constructor();
        }

        override protected void constructor()
        {
            logger.Info("resouce opening...");
            base.constructor();
            logger.Info("resouce opened");
        }

        ~Class1()
        {
            logger.Info("resource closing...");
            destructor();
            logger.Info("resource closed.");
        }
        override public void destructor()
        {
            logger.Info("destructor start");
            base.destructor();
            logger.Info("destructor finish");
        }

        protected bool IsPortInUse()
        {

            logger.Info("IsPortInUse start");
            bool isAvailable = true;

            // Evaluate current system TCP connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == Port)
                {
                    isAvailable = false;
                    break;
                }
            }
            logger.Info("IsPortInUse finished");
            return isAvailable;
        }
        override protected AmazonDynamoDBClient createClient()
        {
            logger.Info("createClient start");

            AmazonDynamoDBClient connection = null;
            var useDynamoDbLocal = true;
            if (useDynamoDbLocal)
            {
                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                var portUsed = IsPortInUse();

                if (portUsed)
                {
                    logger.Warn("The local version of DynamoDBLocal is NOT running.");
                    throw new AmazonDynamoDBException("The local version of DynamoDBLocal is NOT running.");
                }

                // DynamoDB-Local is running, so create a client
                Console.WriteLine( "  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)" );
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = EndpointUrl;
                try
                {
                    logger.Debug("createClient try create AmazonDynamoDBClient in Local Environment");
                    connection = new AmazonDynamoDBClient(ddbConfig);
                }
                catch( Exception ex )
                {
                    logger.Error("createClient failed to create AmazonDynamoDBClient!");
                    logger.Error(ex.Message);
                    throw;
                }
            }
            else
            {
                try
                {
                    logger.Debug("createClient try create AmazonDynamoDBClient in aws Environment");
                    connection = new AmazonDynamoDBClient();
                }catch(Exception ex)
                {
                    logger.Error("createClient failed to create AmazonDynamoDBClient in aws Environment!");
                    logger.Error(ex.Message);
                    throw;
                }
                
            }
            
            logger.Info("createClient finished");
            return connection;
        }
    }
}
