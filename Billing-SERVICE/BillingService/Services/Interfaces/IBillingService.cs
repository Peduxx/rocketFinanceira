using BillingService.Entities;

namespace BillingService.Services.Interfaces
{
    public interface IBillingService
    {
        Task ProcessBilling(Subscription subscription);
        Task ScheduleNextBilling(Subscription subscription);
    }
}
