using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BillingService.Messaging.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQConsumer(string hostname, int port, string username, string password, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                Port = port,
                UserName = username,
                Password = password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
        }

        public void StartConsuming(Action<string> handleMessage)
        {
            _channel.QueueDeclare(queue: _queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handleMessage?.Invoke(message);
            };

            _channel.BasicConsume(queue: _queueName,
                                  autoAck: true,
                                  consumer: consumer);

            Console.WriteLine("Consumer started. Press [enter] to exit.");
            Console.ReadLine();
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
