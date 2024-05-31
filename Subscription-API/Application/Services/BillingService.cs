namespace Application.Services
{
    public class BillingService(IRabbitMQService rabbitMQService) : IBillingService
    {
        private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

        public async Task<bool> ProcessBillingRequest(SubscriptionEntity subscription)
        {
            var result = await _rabbitMQService.PublishMessageAsync("billing-queue", subscription);

            return result;
        }
    }
}
