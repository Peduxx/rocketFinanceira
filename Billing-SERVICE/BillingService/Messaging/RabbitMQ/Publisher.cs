using BillingService.Messaging.RabbitMQ.Interfaces;
using Infrastructure.Messaging.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace BillingService.Messaging
{
    public class Publisher(IConnectionProvider connectionProvider) : IPublisher
    {
        private readonly IConnectionProvider _connectionProvider = connectionProvider;

        public async Task<bool> PublishMessageAsync<T>(string queueName, T message)
        {
            try
            {
                var connection = _connectionProvider.GetConnection();
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
                                     mandatory: false,
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
