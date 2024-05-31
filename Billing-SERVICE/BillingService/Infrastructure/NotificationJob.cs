using Quartz;
using System;
using System.Threading.Tasks;
using BillingService.Entities;
using BillingService.Entities.Enums;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Messaging.RabbitMQ.Interfaces;
using BillingService.Services.Interfaces;

namespace BillingService.Infrastructure
{
    public class NotificationJob : IJob
    {
        private readonly IBillingRepository _billingRepository;
        private readonly IPublisher _publisher;
        private readonly INotificationService _notificationService;

        public NotificationJob(IBillingRepository billingRepository, IPublisher publisher, INotificationService notificationService)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _notificationService = notificationService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("NotificationJob executado.");

                string subscriptionIdString = context.JobDetail.JobDataMap.GetString("SubscriptionId");
                var subscriptionId = Guid.Parse(subscriptionIdString);

                Subscription subscription = _billingRepository.GetSubscriptionById(subscriptionId);

                if (subscription.Status != Status.ACTIVE)
                {
                    Console.WriteLine("Notification not sent. Subscription is not active.");
                    return;
                }

                _notificationService.ProcessNotification(subscription);

                Console.WriteLine("Notification published for subscription ID: " + subscriptionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while executing NotificationJob: {ex.Message}");

                throw;
            }
        }
    }
}
