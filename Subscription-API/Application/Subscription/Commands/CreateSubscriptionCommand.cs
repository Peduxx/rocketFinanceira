namespace Application.Subscription.Commands
{
    public class CreateSubscriptionCommand(Guid idUser) : IRequest<Response>
    {
        public Guid IdUser { get; set; } = idUser;
    }
}