using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTOS;
using Mango.Services.EmailAPI.Service;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly EmailService _emailService; //Cause i registered it as singleton

        private readonly string serviceBusConnectionString; 
        private readonly string emailShoppingCart_Queue; 
        private readonly string registerUserEmail_Queue; 
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailCart_Queue_Processor;
        private ServiceBusProcessor _registerUserEmail_Queue_Processor;

        // topicEmail_subsicription 
        private readonly string OrderCreated_Topic;
        private readonly string OrderCreatedEmail_Subiscription;

        private ServiceBusProcessor _orderCreatedEmail_Topic_Processor;


        public AzureServiceBusConsumer(IConfiguration configuration,EmailService emailService)
        {
            _emailService = emailService;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")!;

            emailShoppingCart_Queue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue")!;
            registerUserEmail_Queue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserEmailQueue")!;

            OrderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic")!;
            OrderCreatedEmail_Subiscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedEmail_Subiscription")!;

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailCart_Queue_Processor = client.CreateProcessor(emailShoppingCart_Queue);
            _registerUserEmail_Queue_Processor = client.CreateProcessor(registerUserEmail_Queue);

            _orderCreatedEmail_Topic_Processor = client.CreateProcessor(OrderCreated_Topic, OrderCreatedEmail_Subiscription);

        }

        public async Task Start() // envoking when api is running
        {
            _emailCart_Queue_Processor.ProcessMessageAsync += OnEmailCart_Queue_RequestRecieved;
            _emailCart_Queue_Processor.ProcessErrorAsync += ErrorHandler;
            await _emailCart_Queue_Processor.StartProcessingAsync();

            _registerUserEmail_Queue_Processor.ProcessMessageAsync += OnRegisterUserEmail_Queue_RequestRecieved;
            _registerUserEmail_Queue_Processor.ProcessErrorAsync += ErrorHandler;
            await _registerUserEmail_Queue_Processor.StartProcessingAsync();

            // subs
            _orderCreatedEmail_Topic_Processor.ProcessMessageAsync += OnOrderCreatdEmail_Topic_RequestRecieved;
            _orderCreatedEmail_Topic_Processor.ProcessErrorAsync += ErrorHandler;
            await _orderCreatedEmail_Topic_Processor.StartProcessingAsync();
        }
        public async Task Stop() // envoking when api is not runnig
        {
            await _emailCart_Queue_Processor.StopProcessingAsync();
            await _emailCart_Queue_Processor.DisposeAsync();

            await _registerUserEmail_Queue_Processor.StopProcessingAsync();
            await _registerUserEmail_Queue_Processor.DisposeAsync();
        }


        private async Task OnOrderCreatdEmail_Topic_RequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            OrderEmailMessage? objMessage = JsonConvert.DeserializeObject<OrderEmailMessage>(body); // convert from json string to model obj

            try
            {
                //TODO: try to log email
                await _emailService.LogOrderPlaced(objMessage);

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private async Task OnEmailCart_Queue_RequestRecieved(ProcessMessageEventArgs args)
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

        private async Task OnRegisterUserEmail_Queue_RequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string emailMsg = JsonConvert.DeserializeObject<string>(body); // convert from json string to model obj

            try
            {
                //TODO: try to log email
                await _emailService.RegisterUserEmailAndLog(emailMsg);

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
