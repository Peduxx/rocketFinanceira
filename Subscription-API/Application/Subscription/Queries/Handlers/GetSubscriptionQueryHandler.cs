using Application.Subscription.Queries.Validators;

namespace Application.Subscription.Queries.Handlers
{
    public class GetSubscriptionQueryHandler(
        GetSubscriptionQueryValidator validator,
        ISubscriptionRepository subscriptionRepository
        ) : IRequestHandler<GetSubscriptionQuery, Response>
    {
        private readonly GetSubscriptionQueryValidator _validator = validator;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;

        public async Task<Response> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Response().SetFailure(validationResult.Errors.FirstOrDefault().ErrorMessage);
                }

                var subscription = await _subscriptionRepository.GetSubscriptionAsync(request.IdUser);

                return new Response().SetSuccess(subscription);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
    }
}

