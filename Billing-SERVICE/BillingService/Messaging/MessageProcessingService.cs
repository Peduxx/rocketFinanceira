using BillingService.Entities;
using BillingService.Infrastructure.Interfaces;
using BillingService.Messaging.RabbitMQ;
using BillingService.Services.Interfaces;
using Infrastructure.Messaging;
using Newtonsoft.Json;

namespace BillingService.Messaging
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private readonly RabbitMQConsumer _rabbitMQConsumer;
        private readonly IBillingService _billingService;
        private readonly INotificationService _notificationService;

        public MessageProcessingService(RabbitMQOptions rabbitMQOptions, IBillingService billingService, INotificationService notificationService)
        {
            _rabbitMQConsumer = new RabbitMQConsumer(rabbitMQOptions.Hostname, rabbitMQOptions.Port, rabbitMQOptions.Username, rabbitMQOptions.Password, "billing-queue");
            _billingService = billingService;
            _notificationService = notificationService;
        }

        public void StartConsuming(string queueName)
        {
            _rabbitMQConsumer.StartConsuming(message => ProcessMessage(message, queueName));
        }

        private async void ProcessMessage(string message, string queueName)
        {
            Console.WriteLine($"Received message from queue '{queueName}': {message}");

            var subscription = JsonConvert.DeserializeObject<Subscription>(message);
            var processedSubscription = ProcessSubscription(subscription);

            if (queueName == "billing-queue")
            {
                await _billingService.ProcessBilling(processedSubscription);
            }
            else if (queueName == "notification-queue")
            {
                await _notificationService.ProcessNotification(processedSubscription);
            }
        }

        private static Subscription ProcessSubscription(Subscription subscription)
        {
            return new Subscription(subscription.Id, subscription.IdUser, subscription.BillingDate, subscription.Status);
        }
    }
}
