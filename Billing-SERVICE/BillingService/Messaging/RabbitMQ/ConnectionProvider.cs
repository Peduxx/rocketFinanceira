using Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly RabbitMQOptions _rabbitMQOptions;
        private readonly IConnection _connection;

        public ConnectionProvider(RabbitMQOptions rabbitMQOptions)
        {
            _rabbitMQOptions = rabbitMQOptions;
            _connection = CreateConnection();
        }

        public IConnection GetConnection()
        {
            return _connection;
        }

        private IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQOptions.Hostname,
                Port = _rabbitMQOptions.Port,
                UserName = _rabbitMQOptions.Username,
                Password = _rabbitMQOptions.Password,
                VirtualHost = _rabbitMQOptions.VirtualHost
            };

            return factory.CreateConnection();
        }
    }
}
