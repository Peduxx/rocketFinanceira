namespace Application.User.Commands.Handlers
{
    public class SignInCommandHandler(
        IJwtProvider jwtProvider,
        SignInCommandValidator validator,
        IUserRepository userRepository
        ) : IRequestHandler<SignInCommand, Response>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly SignInCommandValidator _validator = validator;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Response> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Response().SetFailure(validationResult.Errors.FirstOrDefault().ErrorMessage);
                }

                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user == null)
                    return new Response().SetFailure("Your email or password is incorrect.");

                var password = await _userRepository.GetPasswordByIdUserAsync(user.Id);
                var salt = await _userRepository.GetSaltByIdUserAsync(user.Id);

                var passwordInstance = new Password { Value = password, Salt = salt };
                var checkPassword = passwordInstance.Compare(request.Password);

                if (!checkPassword)
                    return new Response().SetFailure("Your email or password is incorrect.");

                var token = await _jwtProvider.GenerateAsync(user);

                return new Response().SetSuccess(token);
            }
            catch(Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
