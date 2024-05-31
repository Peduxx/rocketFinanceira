namespace Application.Subscription.Commands.Handlers
{
    public class CancelSubscriptionCommandHandler(
        CancelSubscriptionCommandValidator validator,
        ISubscriptionRepository subscriptionRepository
        ) : IRequestHandler<CancelSubscriptionCommand, Response>
    {
        private readonly CancelSubscriptionCommandValidator _validator = validator;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;

        public async Task<Response> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Response().SetFailure(validationResult.Errors.FirstOrDefault().ErrorMessage);
                }

                var alreadySub = await _subscriptionRepository.GetActiveSubscriptionAsync(request.IdUser);

                if (!alreadySub)
                {
                    return new Response().SetFailure("Active subscription was not found.");
                }

                await _subscriptionRepository.CancelAsync(request.IdUser);

                return new Response().SetSuccess("Your subscription was successfull canceled.");
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}