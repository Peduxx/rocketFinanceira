using BillingService.Entities.Enums;

namespace BillingService.Entities
{
    public class Subscription(
        Guid id,
        Guid idUser,
        DateTime billingDate,
        Status status
        )
    {
        public Guid Id { get; set; } = id;
        public Guid IdUser { get; set; } = idUser;
        public DateTime BillingDate { get; set; } = billingDate;
        public Status Status { get; set; } = status;
    }
}
