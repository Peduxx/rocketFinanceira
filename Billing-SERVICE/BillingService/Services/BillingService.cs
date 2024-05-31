using BillingService.Entities;
using BillingService.Infrastructure;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Infrastructure.Email;
using BillingService.Messaging.RabbitMQ.Interfaces;
using BillingService.Services.Interfaces;
using Quartz;
using Quartz.Impl;

namespace BillingService.Services
{
    public class BillingService : IBillingService
    {
        private readonly IScheduler _scheduler;
        private readonly IBillingRepository _billingRepository;
        private readonly IEmailService _emailService;
        private readonly IPublisher _publisher;
        private readonly INotificationService _notificationService;

        public BillingService(IScheduler scheduler, IBillingRepository billingRepository, IEmailService emailService, IPublisher publisher, INotificationService notificationService)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));

            StdSchedulerFactory stdSchedulerFactory = new StdSchedulerFactory();
            _scheduler = stdSchedulerFactory.GetScheduler().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler obtido.");

            _scheduler.Start().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler iniciado.");

            Console.WriteLine("Configurações do scheduler:");
            Console.WriteLine($"SchedulerName: {_scheduler.SchedulerName}");
            _emailService = emailService;
            _publisher = publisher;
            _notificationService = notificationService;
        }

        //private async Task StartNotificationQueue(Subscription subscription)
        //{
        //    await _publisher.PublishMessageAsync("notification-queue", "Starting queue");
        //}

        public async Task ProcessBilling(Subscription subscription)
        {
            // StartNotificationQueue(subscription);

            Console.WriteLine($"Processing billing for subscription: {subscription}");

            Console.WriteLine("Billing processed.");

            var user = _billingRepository.GetUserById(subscription.IdUser);

            await _emailService.SendEmailAsync(user.Email, "Payment receipt.", "Thank you!");

            subscription = _billingRepository.UpdateBillingDate(subscription);

            //Agendar proxima notificação

            await _notificationService.ScheduleNextNotification(subscription);
            await ScheduleNextBilling(subscription);
        }

        public async Task ScheduleNextBilling(Subscription subscription)
        {
            DateTimeOffset startTime = DateTimeOffset.Now.AddDays(30);

            IJobDetail job = JobBuilder.Create<PaymentJob>()
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
