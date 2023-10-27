using Azure.Messaging.ServiceBus;
using Mango.Services.RewardAPI.Models;
using Mango.Services.RewardAPI.Service;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString; 
        private readonly string orderCreatedTopic; 
        private readonly string orderCreatedRewardsSubiscription; 
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _rewardUpdateProcessor;

        private readonly RewardService _rewardsService; //Cause i registered it as singleton

        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {
            _rewardsService = rewardService;
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString")!;

            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic")!;
            orderCreatedRewardsSubiscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedRewards_Subiscription")!;

            var client = new ServiceBusClient(serviceBusConnectionString);

            _rewardUpdateProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardsSubiscription);

        }

        public async Task Start() // envoking when api is running
        {
            _rewardUpdateProcessor.ProcessMessageAsync += OnOrderRewardRequestRecieved;
            _rewardUpdateProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardUpdateProcessor.StartProcessingAsync();
        }

        public async Task Stop() // envoking when api is not runnig
        {
            await _rewardUpdateProcessor.StopProcessingAsync();
            await _rewardUpdateProcessor.DisposeAsync();
        }
        private async Task OnOrderRewardRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage? objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body); // convert from json string to model obj

            try
            {
                //TODO: try to log email

                await _rewardsService.UpdateRewards(objMessage);

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
