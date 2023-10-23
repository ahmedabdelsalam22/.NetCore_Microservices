using Mango.Services.EmailAPI.Messaging;

namespace Mango.Services.EmailAPI.Extenstions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer serviceBusConsumer {  get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app) 
        {
            serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();

            var hostApplciationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplciationLife.ApplicationStarted.Register(OnStart);
            hostApplciationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            serviceBusConsumer.Start();
        }
        private static void OnStop()
        {
            serviceBusConsumer.Stop();
        }

        
    }
}
