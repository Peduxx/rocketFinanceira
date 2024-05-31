namespace Application.User.Commands
{
    public class SignUpCommand(
        string name,
        string email,
        string password
        ) : IRequest<Response>
    {
        public string Name { get; set; } = name;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;
    }
}
