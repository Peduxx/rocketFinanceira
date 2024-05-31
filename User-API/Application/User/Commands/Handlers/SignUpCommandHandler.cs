namespace Application.User.Commands.Handlers
{
    public class SignUpCommandHandler(
        IUserRepository userRepository,
        SignUpCommandValidator validator,
        IJwtProvider jwtProvider
        ) : IRequestHandler<SignUpCommand, Response>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly SignUpCommandValidator _validator = validator;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<Response> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Response().SetFailure(validationResult.Errors.FirstOrDefault().ErrorMessage);
                }

                var user = await _userRepository.GetByEmailAsync(request.Email);

                if (user != null)
                    return new Response().SetFailure("This email already exists.");

                user = UserEntity.Create(request.Name, request.Email, request.Password);

                await _userRepository.SaveAsync(user);

                var token = await _jwtProvider.GenerateAsync(user);

                return new Response().SetSuccess(token, "User successfully created.");
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
