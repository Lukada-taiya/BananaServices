
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Banana.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string connectionString = "--Add Azure Service Bus";
        public async Task Publish(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);
            var serializedmessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(serializedmessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(serviceBusMessage);
            await client.DisposeAsync();

        }
    }
}
