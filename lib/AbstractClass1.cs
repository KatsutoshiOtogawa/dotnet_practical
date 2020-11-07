using System;
using NLog;
using System.Net;
using System.Net.NetworkInformation;
using Amazon.DynamoDBv2;
namespace lib
{

    public  abstract class AbstractClass1 : IClass1
    {
        protected string Ip;
        protected int Port;
        protected string EndpointUrl;
        
        virtual public void destructor()
        {
            // logger.Info("destructor start");

            // logger.Info("destructor finish");
        }

        protected void constructor()
        {
            createClient();
        }

        protected bool IsPortInUse()
        {
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

            return isAvailable;
        }

        protected bool createClient()
        {
            var useDynamoDbLocal = true;
            if (useDynamoDbLocal)
            {
                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                var portUsed = IsPortInUse();

                if (portUsed)
                {
                    Console.WriteLine("The local version of DynamoDB is NOT running.");
                    return (false);
                }

                // DynamoDB-Local is running, so create a client
                Console.WriteLine( "  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)" );
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = EndpointUrl;
                try
                {
                    var Client = new AmazonDynamoDBClient(ddbConfig);
                }
                catch( Exception ex )
                {
                Console.WriteLine( "     FAILED to create a DynamoDBLocal client; " + ex.Message );
                return false;
                }
            }
            else
            {
                var lient = new AmazonDynamoDBClient();
            }
            
            return true;
        }
    }
}
