namespace Application.Subscription.Commands.Validators
{
    public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionCommandValidator()
        {
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("IdUser required.");
        }
    }
}
