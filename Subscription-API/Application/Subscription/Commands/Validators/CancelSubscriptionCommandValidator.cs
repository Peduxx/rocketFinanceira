namespace Application.Subscription.Commands.Validators
{
    public class CancelSubscriptionCommandValidator : AbstractValidator<CancelSubscriptionCommand>
    {
        public CancelSubscriptionCommandValidator()
        {
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("IdUser required.");
        }
    }
}
