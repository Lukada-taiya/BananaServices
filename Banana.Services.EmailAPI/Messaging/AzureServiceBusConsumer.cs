using Azure.Messaging.ServiceBus;
using Banana.Services.EmailAPI.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace Banana.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusProcessor _serviceBusProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCartQueue");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _serviceBusProcessor = client.CreateProcessor(emailCartQueue); 
        }

        public async Task Start() 
        {
            _serviceBusProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _serviceBusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cart = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await args.CompleteMessageAsync(message);
            }catch(Exception e)
            { 
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //Possible send email own email for debugging
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
