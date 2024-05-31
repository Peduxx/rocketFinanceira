namespace Application.Services.Interfaces
{
    public interface IBillingService
    {
        Task<bool> ProcessBillingRequest(SubscriptionEntity subscription);
    }
}
