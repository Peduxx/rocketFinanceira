namespace BillingService.Infrastructure.Interfaces
{
    public interface IMessageProcessingService
    {
        void StartConsuming(string queueName);
    }
}
