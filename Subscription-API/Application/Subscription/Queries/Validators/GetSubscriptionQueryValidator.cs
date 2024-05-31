namespace Application.Subscription.Queries.Validators
{
    public class GetSubscriptionQueryValidator : AbstractValidator<GetSubscriptionQuery>
    {
        public GetSubscriptionQueryValidator()
        {
            RuleFor(x => x.IdUser).NotEmpty().WithMessage("IdUser required.");
        }
    }
}
