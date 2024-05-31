using Application.Services;
using Application.Services.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Interfaces;

namespace Infrastructure.CrossCutting.IoC
{
    public static class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Validators
            services.AddScoped<CreateSubscriptionCommandValidator>();
            services.AddScoped<CancelSubscriptionCommandValidator>();
            services.AddScoped<GetSubscriptionQueryValidator>();

            // Providers
            services.AddSingleton<IConnectionProvider, ConnectionProvider>();

            // Messaging
            services.AddScoped<IRabbitMQService, RabbitMQService>();
            services.AddScoped<IBillingService, BillingService>();
            services.AddSingleton<RabbitMQOptions>();

            // Data
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        }
    }
}