namespace BillingService.Messaging.RabbitMQ.Interfaces
{
    public interface IPublisher
    {
        public Task<bool> PublishMessageAsync<T>(string queueName, T message);
    }
}
