using BillingService.Entities;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Data.Repositories
{
    public class BillingRepository(DataContext dataContext) : IBillingRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public Subscription GetSubscriptionById(Guid id)
        {
            var activeSubscription = _dataContext.Subscription.FirstOrDefault(s => s.Id == id);

            return activeSubscription;
        }

        public User GetUserById(Guid id)
        {
            var user = _dataContext.User.FirstOrDefault(u => u.Id == id);

            return user;
        }

        public Subscription UpdateBillingDate(Subscription subscription)
        {
            subscription.BillingDate = subscription.BillingDate.AddDays(30);

            _dataContext.SaveChanges();

            return subscription;
        }
    }
}
