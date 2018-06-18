using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using ForexDatabase.DAL;
using Model;
using Quartz;
using Quartz.Impl;

namespace RecordsPublisher
{
    class Scheduler
    {
        async public void run(int frequencyInSeq)
        {
            Console.WriteLine("Scheduler launched.");
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            IScheduler sched = await factory.GetScheduler();
            await sched.Start();

            IJobDetail job = JobBuilder.Create<PublisherJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(frequencyInSeq)
                  .RepeatForever())
              .Build();

            await sched.ScheduleJob(job, trigger);

            while (true)
            {

            }
        }

        static void Main(string[] args)
        {
            new Scheduler().run(5);
        }
    }

    class PublisherJob : IJob
    {
        private static readonly string USERS_TOPIC = "user-log";
        private static readonly string TRANSACTIONS_TOPIC = "transaction-log";
        private static readonly Dictionary<string, object> kafkaConfig = new Dictionary<string, object>
        {
            { "bootstrap.servers", "192.168.43.70:9092" }
        };

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Task launched.");

            string usersCount = "0";
            string transactionsCount = "0";

            using (var producer = new Producer<Null, string>(kafkaConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("Attempting to write data: users - " + usersCount + ", transactions - " + transactionsCount);
                var usersResult = producer.ProduceAsync(USERS_TOPIC, null, usersCount).Result;
                var transactionsResult = producer.ProduceAsync(TRANSACTIONS_TOPIC, null, transactionsCount).Result;
                await Console.Out.WriteLineAsync("Got data: users - " + usersResult + ", transactions - " + transactionsResult);
            }
        }
    }
}
