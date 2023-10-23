using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.DTOS;
using Mango.Services.EmailAPI.Service;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString; 
        private readonly string emailShoppingCartQueue; 
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailCartProcessor;

        private readonly EmailService _emailService; //Cause i registered it as singleton

        public AzureServiceBusConsumer(IConfiguration configuration,EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailShoppingCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")!;


            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(emailShoppingCartQueue);

        }

        public async Task Start() // envoking when api is running
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestRecieved;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }
        public async Task Stop() // envoking when api is not runnig
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }
        private async Task OnEmailCartRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto? objMessage = JsonConvert.DeserializeObject<CartDto>(body); // convert from json string to model obj

            try
            {
                //TODO: try to log email
                await _emailService.EmailCartAndLog(objMessage);

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

       
    }
}
