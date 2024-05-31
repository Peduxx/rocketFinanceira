using Domain.Entities;

namespace Domain.Ports
{
    public interface ISubscriptionRepository
    {
        Task CreateAsync(Subscription subscription);
        Task CancelAsync(Guid subscriptionId);
        Task<Subscription> GetSubscriptionAsync(Guid idUser);
        Task<bool> GetActiveSubscriptionAsync(Guid idUser);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
