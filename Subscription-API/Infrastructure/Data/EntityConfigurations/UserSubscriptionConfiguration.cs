namespace Infrastructure.Data.EntityConfigurations
{
    public class UserSubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscription");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.IdUser)
                   .HasColumnName("IdUser")
                   .IsRequired();
        }
    }
}
