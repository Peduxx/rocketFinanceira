namespace Application.User.Commands
{
    public class SignInCommand(
        string email,
        string password
        ) : IRequest<Response>
    {
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}
