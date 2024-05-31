namespace Api.OptionsSetup
{
    public static class DatabaseOptionsSetup
    {
        private const string SectionName = "DefaultConnection";

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(SectionName)));

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(SectionName)));
        }
    }
}