namespace Api.OptionsSetup
{
    public static class DependencyInjectionOptionsSetup
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            NativeInjector.RegisterServices(services);
        }
    }
}