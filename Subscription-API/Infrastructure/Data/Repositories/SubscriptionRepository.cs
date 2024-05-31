namespace Infrastructure.Data.Repositories
{
    public class SubscriptionRepository(DataContext context) : ISubscriptionRepository
    {
        private readonly DataContext _context = context;
        private IDbContextTransaction _transaction;

        public async Task CreateAsync(Subscription subscription)
        {
            _context.Subscription.Add(subscription);
        }

        public async Task CancelAsync(Guid idUser)
        {
            var subscription = await _context.Subscription.FirstOrDefaultAsync(s => s.IdUser == idUser);

            subscription.Status = Status.INACTIVE;
            subscription.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<Subscription> GetSubscriptionAsync(Guid idUser)
        {
            var subscription = await _context.Subscription
                .FirstOrDefaultAsync(s => s.IdUser == idUser);

            return subscription;
        }

        public async Task<bool> GetActiveSubscriptionAsync(Guid idUser)
        {
            var activeSubscription = await _context.Subscription
                .FirstOrDefaultAsync(s => s.IdUser == idUser && s.Status == Status.ACTIVE);

            return activeSubscription != null;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await _transaction.RollbackAsync();
        }
    }
}
