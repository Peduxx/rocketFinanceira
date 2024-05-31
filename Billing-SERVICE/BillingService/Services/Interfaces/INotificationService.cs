using BillingService.Entities;

namespace BillingService.Services.Interfaces
{
    public interface INotificationService
    {
        Task ProcessNotification(Subscription subscription);
        Task ScheduleNextNotification(Subscription subscription);
    }
}
