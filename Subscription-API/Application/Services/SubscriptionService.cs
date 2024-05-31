namespace Application.Services
{
    public class SubscriptionService(IRabbitMQService rabbitMQService)
    {
        private readonly IRabbitMQService _rabbitMQService = rabbitMQService;

        public async Task ProcessCancelSubscriptionRequest(SubscriptionEntity subscription)
        {
            await _rabbitMQService.PublishMessageAsync("subscription-queue", subscription);
        }
    }
}
