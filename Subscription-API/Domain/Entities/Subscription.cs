namespace Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public Guid IdUser { get; set; }
        public DateTime BillingDate { get; set; }
        public Status Status { get; set; }

        public static async Task<Subscription> Create(Guid idUser)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                IdUser = idUser,
                BillingDate = DateTime.Now,
                Status = Status.ACTIVE,
                CreatedAt = DateTime.Now
            };
        }
    }
}
