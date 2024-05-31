namespace Application.User.Commands
{
    public class UpdateUserCommand(
        string email,
        string newEmail
        ) : IRequest<Response>
    {
        public string Email { get; set; } = email;
        public string NewEmail { get; set; } = newEmail;
    }
}