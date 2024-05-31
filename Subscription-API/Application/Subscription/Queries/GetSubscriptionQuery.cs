namespace Application.Subscription.Queries
{
    public class GetSubscriptionQuery(Guid idUser) : IRequest<Response>
    {
        public Guid IdUser { get; set; } = idUser;
    }
}
