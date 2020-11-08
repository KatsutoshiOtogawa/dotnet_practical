using NLog;
using System.Net;
using System.Net.NetworkInformation;
namespace lib
{

    public  abstract class AbstractClass1<T>
    {
        protected string Ip;
        protected int Port;
        protected string EndpointUrl;
        protected T dbclient;
        private static Logger AbstractLogger = LogManager.GetCurrentClassLogger();

        protected virtual void constructor()
        {
            AbstractLogger.Debug("constructor start");
            dbclient = createClient();
            AbstractLogger.Debug("constructor finish");
        }
        public virtual void destructor()
        {
            AbstractLogger.Debug("destructor start");

            AbstractLogger.Debug("destructor finish");
        }

        protected abstract T createClient();
    }
}
