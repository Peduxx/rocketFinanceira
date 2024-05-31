namespace Infrastructure.CrossCutting.IoC
{
    public static class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Validators
            services.AddScoped<SignUpCommandValidator>();
            services.AddScoped<SignInCommandValidator>();
            services.AddScoped<UpdateUserCommandValidator>();

            // Providers
            services.AddScoped<IJwtProvider, JwtProvider>();

            // Data
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}