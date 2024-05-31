using System;
using System.Threading.Tasks;
using BillingService.Entities;
using BillingService.Infrastructure;
using BillingService.Infrastructure.Data.Repositories.Interfaces;
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
        private readonly IPublisher _publisher;

        public BillingService(IScheduler scheduler, IBillingRepository billingRepository, IPublisher publisher)
        {
            _billingRepository = billingRepository ?? throw new ArgumentNullException(nameof(billingRepository));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

            StdSchedulerFactory stdSchedulerFactory = new StdSchedulerFactory();
            _scheduler = stdSchedulerFactory.GetScheduler().GetAwaiter().GetResult();

            // Log para verificar se o scheduler foi obtido corretamente
            Console.WriteLine("Scheduler obtido.");

            _scheduler.Start().GetAwaiter().GetResult();

            // Log para verificar se o scheduler foi iniciado corretamente
            Console.WriteLine("Scheduler iniciado.");

            Console.WriteLine("Configurações do scheduler:");
            Console.WriteLine($"SchedulerName: {_scheduler.SchedulerName}");
        }

        public async Task ProcessBilling(Subscription subscription)
        {
            Console.WriteLine($"Processing billing for subscription: {subscription}");

            // Simula o processamento da cobrança
            Console.WriteLine("Billing processed.");


            //enviar comprovante
            subscription = _billingRepository.UpdateBillingDate(subscription);

            // Agendar a próxima cobrança
            await ScheduleNextBilling(subscription);
        }

        public async Task ScheduleNextBilling(Subscription subscription)
        {
            DateTimeOffset startTime = DateTimeOffset.Now.AddMinutes(1); // Agendar para daqui a 2 minutos

            // Passando as dependências para o construtor do PaymentJob
            IJobDetail job = JobBuilder.Create<PaymentJob>()
                //.WithIdentity($"PaymentJob_{subscription.Id}_{subscription.BillingDate}")
                .UsingJobData("SubscriptionId", subscription.Id.ToString())
                .Build();

            // Agendando trigger para o PaymentJob
            ITrigger trigger = TriggerBuilder.Create()
                //.WithIdentity($"PaymentTrigger_{subscription.Id}_{subscription.BillingDate}")
                .StartAt(DateTime.Now.AddMinutes(1))
                .Build();

            // Agendando o trabalho
            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
