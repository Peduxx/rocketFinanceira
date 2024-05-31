using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using BillingService.Entities;
using BillingService.Infrastructure.Data;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Infrastructure.Data.Repositories;
using BillingService.Infrastructure.Email;
using BillingService.Messaging.RabbitMQ.Interfaces;
using BillingService.Messaging.RabbitMQ;
using BillingService.Services.Interfaces;
using BillingService.Services;
using Infrastructure.Messaging.Interfaces;
using Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using BillingService.Messaging;
using BillingService.Infrastructure;
using Quartz.Impl;
using Quartz;
using BillingService.JobConfig;
using Quartz.Spi;
using BillingService.Infrastructure.Interfaces;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var messageProcessingService = serviceProvider.GetService<IMessageProcessingService>();

            messageProcessingService.StartConsuming("billing-queue");

            messageProcessingService.StartConsuming("notification-queue");

            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }


        static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton(provider =>
            {
                var scheduler = new StdSchedulerFactory().GetScheduler().GetAwaiter().GetResult();
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                return scheduler;
            });

            services.AddScoped<IBillingService, BillingService.Services.BillingService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IBillingRepository, BillingRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPublisher, Publisher>();
            services.AddScoped<IConnectionProvider, ConnectionProvider>();
            services.AddScoped<IMessageProcessingService, MessageProcessingService>();

            services.AddTransient<PaymentJob>();
            services.AddTransient<NotificationJob>();

            services.AddScoped(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer("Server=localhost,1433;Database=user-database;User ID=sa;Password=1q2w3e4r@#$;TrustServerCertificate=true");
                return new DataContext(optionsBuilder.Options);
            });

            services.AddSingleton<RabbitMQOptions>();

            return services.BuildServiceProvider();
        }
    }
}
