using Application.Subscription.Commands;

public class CreateSubscriptionCommandHandler(
    ISubscriptionRepository subscriptionRepository,
    IBillingService billingService,
    CreateSubscriptionCommandValidator validator
    ) : IRequestHandler<CreateSubscriptionCommand, Response>
{
    private readonly CreateSubscriptionCommandValidator _validator = validator;
    private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
    private readonly IBillingService _billingService = billingService;

    public async Task<Response> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await _subscriptionRepository.BeginTransactionAsync();

        try
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new Response().SetFailure(validationResult.Errors.FirstOrDefault().ErrorMessage);
            }

            var alreadySub = await _subscriptionRepository.GetActiveSubscriptionAsync(request.IdUser);

            if (alreadySub)
            {
                return new Response().SetFailure("You already have one subscription.");
            }

            var subscription = await SubscriptionEntity.Create(request.IdUser);

            await _subscriptionRepository.CreateAsync(subscription);

            var messageSent = await _billingService.ProcessBillingRequest(subscription);

            if(!messageSent)
            {
                await _subscriptionRepository.RollbackTransactionAsync();
                return new Response().SetFailure("Error while trying to send billing. Try again.");
            }

            await _subscriptionRepository.CommitTransactionAsync();

            return new Response().SetSuccess("Your subscription was successfully created.");
        }
        catch (Exception ex)
        {
            await _subscriptionRepository.RollbackTransactionAsync();
            throw new(ex.Message);
        }
    }
}
