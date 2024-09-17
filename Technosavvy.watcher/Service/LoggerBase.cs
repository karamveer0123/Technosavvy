using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace NavExM.Int.Watcher.WatchDog.Service
{
    internal abstract class LoggerBase
    {
       
        internal protected Tuple<bool, string> Ok(bool result, string Msg)
        {
            return new Tuple<bool, string>(result, Msg);
        }
       
    }
}
