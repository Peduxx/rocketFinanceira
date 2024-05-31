using Infrastructure.Messaging.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Messaging
{
    public class RabbitMQService(IConnectionProvider connectionProvider) : IRabbitMQService
    {
        private readonly IConnectionProvider _connectionProvider = connectionProvider;

        public async Task<bool> PublishMessageAsync<T>(string queueName, T message)
        {
            try
            {
                using var connection = _connectionProvider.GetConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
                return false;
            }
        }
    }
}
