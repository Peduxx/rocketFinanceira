using BillingService.Entities;
using BillingService.Infrastructure;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
using BillingService.Services.Interfaces;
using Quartz;
using Quartz.Impl;

namespace BillingService.Services
{
    public class BillingService : IBillingService
    {
        private readonly IScheduler _scheduler;
        private readonly IBillingRepository _billingRepository;

        public BillingService(IScheduler scheduler, IBillingRepository billingRepository)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));

            StdSchedulerFactory stdSchedulerFactory = new StdSchedulerFactory();
            _scheduler = stdSchedulerFactory.GetScheduler().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler obtido.");

            _scheduler.Start().GetAwaiter().GetResult();

            Console.WriteLine("Scheduler iniciado.");

            Console.WriteLine("Configurações do scheduler:");
            Console.WriteLine($"SchedulerName: {_scheduler.SchedulerName}");
        }

        public async Task ProcessBilling(Subscription subscription)
        {
            Console.WriteLine($"Processing billing for subscription: {subscription}");

            Console.WriteLine("Billing processed.");

            subscription = _billingRepository.UpdateBillingDate(subscription);

            await ScheduleNextBilling(subscription);
        }

        public async Task ScheduleNextBilling(Subscription subscription)
        {
            DateTimeOffset startTime = DateTimeOffset.Now.AddMinutes(1);

            IJobDetail job = JobBuilder.Create<PaymentJob>()
                //.WithIdentity($"PaymentJob_{subscription.Id}_{subscription.BillingDate}")
                .UsingJobData("SubscriptionId", subscription.Id.ToString())
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                //.WithIdentity($"PaymentTrigger_{subscription.Id}_{subscription.BillingDate}")
                .StartAt(DateTime.Now.AddMinutes(1))
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
