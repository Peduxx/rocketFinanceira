namespace Application.Subscription.Commands
{
    public class CancelSubscriptionCommand(Guid idUser) : IRequest<Response>
    {
        public Guid IdUser { get; set; } = idUser;
    }
}
