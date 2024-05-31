using BillingService.Entities;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Infrastructure.Email;
using BillingService.Services.Interfaces;
using Quartz.Impl;
using Quartz;
using BillingService.Infrastructure;
using BillingService.Messaging.RabbitMQ.Interfaces;

namespace BillingService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IScheduler _scheduler;
        private readonly IBillingRepository _billingRepository;
        private readonly IEmailService _emailService;

        public NotificationService(IScheduler scheduler, IBillingRepository billingRepository, IEmailService emailService, IPublisher publisher)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));

            StdSchedulerFactory stdSchedulerFactory = new();
            _scheduler = stdSchedulerFactory.GetScheduler().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler obtido.");

            _scheduler.Start().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler iniciado.");
             
            Console.WriteLine("Configurações do scheduler:");
            Console.WriteLine($"SchedulerName: {_scheduler.SchedulerName}");

            _emailService = emailService;
        }

        public async Task ProcessNotification(Subscription subscription)
        {
            var user = _billingRepository.GetUserById(subscription.IdUser);

            await _emailService.SendEmailAsync(user.Email, "Billing tomorrow.", "Thank you!");
        }

        public async Task ScheduleNextNotification(Subscription subscription)
        {
            DateTimeOffset startTime = DateTimeOffset.Now.AddDays(29);

            IJobDetail job = JobBuilder.Create<NotificationJob>()
                //.WithIdentity($"PaymentJob_{subscription.Id}_{subscription.BillingDate}")
                .UsingJobData("SubscriptionId", subscription.Id.ToString())
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                //.WithIdentity($"PaymentTrigger_{subscription.Id}_{subscription.BillingDate}")
                .StartAt(startTime)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}