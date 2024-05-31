using RabbitMQ.Client;

namespace Infrastructure.Messaging.Interfaces
{
    public interface IConnectionProvider
    {
        IConnection GetConnection();
    }
}
