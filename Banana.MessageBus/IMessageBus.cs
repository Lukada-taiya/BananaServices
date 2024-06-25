using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.MessageBus
{
    public interface IMessageBus
    {
        Task Publish(object  message, string topic_queue_name, string connection_string);
    }
}
