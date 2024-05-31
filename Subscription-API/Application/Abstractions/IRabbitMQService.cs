namespace Application.Abstractions
{
    public interface IRabbitMQService
    {
        public Task<bool> PublishMessageAsync<T>(string queueName, T message);
    }
}
