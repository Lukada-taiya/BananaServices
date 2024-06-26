using Azure.Messaging.ServiceBus;
using Banana.Services.EmailAPI.Models.Dto;
using Banana.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Banana.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailServiceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string emailLogQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly ServiceBusProcessor _serviceBusProcessor;
        private readonly ServiceBusProcessor _emailServiceBusProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("MessageBusConnString");
            emailServiceBusConnectionString = _configuration.GetValue<string>("EmailServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicsAndQueueNames:UserEmailLogQueue");
            emailLogQueue = _configuration.GetValue<string>("TopicsAndQueueNames:EmailShoppingCartQueue");
            var client = new ServiceBusClient(serviceBusConnectionString); 
            _serviceBusProcessor = client.CreateProcessor(emailCartQueue); 
            _emailServiceBusProcessor = client.CreateProcessor(emailCartQueue); 
            _emailService = emailService;

        }

        public async Task Start() 
        {
            _serviceBusProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _serviceBusProcessor.StartProcessingAsync();

            _emailServiceBusProcessor.ProcessMessageAsync += OnUserEmailRequestReceived;
            _emailServiceBusProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailServiceBusProcessor.StartProcessingAsync();
        }


        public async Task Stop()
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
            
            await _emailServiceBusProcessor.StopProcessingAsync();
            await _emailServiceBusProcessor.DisposeAsync();
        }

        private async Task OnUserEmailRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cart = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await _emailService.EmailCartAndLog(cart);
                await args.CompleteMessageAsync(message);
            }catch(Exception)
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
