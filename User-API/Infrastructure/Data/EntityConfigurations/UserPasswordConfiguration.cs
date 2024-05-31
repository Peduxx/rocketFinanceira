namespace Infrastructure.Data.EntityConfigurations
{
    public class UserPasswordConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(u => u.Password)
                .WithOne()
                .HasForeignKey<Password>(p => p.IdUser);
        }
    }
}
 