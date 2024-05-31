namespace Application.User.Commands.Handlers
{
    public class UpdateUserCommandHandler(
        UpdateUserCommandValidator validator,
        IUserRepository userRepository
        ) : IRequestHandler<UpdateUserCommand, Response>
    {
        private readonly UpdateUserCommandValidator _validator = validator;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Response> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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
                    return new Response().SetFailure("You are not registered.");

                await _userRepository.UpdateAsync(request.Email, request.NewEmail);

                return new Response().SetSuccess(null, "Successfully updated.");
            }
            catch(Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}
