using BillingService.Entities;

namespace BillingService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IBillingRepository
    {
        Subscription GetSubscriptionById(Guid id);
        Subscription UpdateBillingDate(Subscription subscription);
        User GetUserById(Guid id);
    }
}
