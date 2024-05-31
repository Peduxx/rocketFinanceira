using BillingService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<User> User { get; set; }
    }
}
