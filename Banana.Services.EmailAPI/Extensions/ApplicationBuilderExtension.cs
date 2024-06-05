using Banana.Services.EmailAPI.Messaging;

namespace Banana.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtension
    {
        private static IAzureServiceBusConsumer _consumer;
        public static IApplicationBuilder UseAzureServiceConsumer(this IApplicationBuilder app)
        {
            _consumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostlifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostlifetime.ApplicationStarted.Register(OnStart);
            hostlifetime.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            _consumer.Start();
        }

        private static void OnStop()
        {
            _consumer.Stop();
        }
    }
}
