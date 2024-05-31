using BillingService.Entities;
using BillingService.Entities.Enums;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Messaging.RabbitMQ.Interfaces;
using BillingService.Services.Interfaces;
using Quartz;

namespace BillingService.Infrastructure
{
    public class PaymentJob : IJob
    {
        private readonly IBillingService _billingService;
        private readonly IBillingRepository _billingRepository;
        private readonly IPublisher _publisher;

        public PaymentJob(IBillingRepository billingRepository, IPublisher publisher, IBillingService billingService)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _billingService = billingService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("PaymentJob executado.");

                string subscriptionIdString = context.JobDetail.JobDataMap.GetString("SubscriptionId");
                var subscriptionId = Guid.Parse(subscriptionIdString);

                Console.WriteLine($"Executing payment job for subscription ID: {subscriptionId}");

                Subscription subscription = _billingRepository.GetSubscriptionById(subscriptionId);

                if (subscription.Status != Status.ACTIVE)
                {
                    Console.WriteLine("Billing not processed. Subscription is not active.");
                    return;
                }

                await _billingService.ProcessBilling(subscription);

                await _publisher.PublishMessageAsync("billing-queue", subscription);
            }
            catch (Exception ex)
            {
                // Registre informações sobre a exceção, como mensagens de log
                Console.WriteLine($"An error occurred while executing PaymentJob: {ex.Message}");

                // Se desejar, lance a exceção novamente para que o Quartz.NET possa tratá-la adequadamente
                throw;
            }
        }
    }
}
