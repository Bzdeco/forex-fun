using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using ForexDatabase.DAL;
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
        private static readonly string USERS_TOPIC = "060yyody-users";
        private static readonly string TRANSACTIONS_TOPIC = "060yyody-transactions";
        private static readonly string JAAS_CONFIG = String.Format("org.apache.kafka.common.security.scram.ScramLoginModule required username=\"{0}\" password=\"{1}\";",
            "060yyody", "QqZNAE-Z5paRREQqOrrnWIFB9WQv7LVC");
        private static readonly Dictionary<string, object> kafkaConfig = new Dictionary<string, object>
        {
            { "bootstrap.servers", "ark-01.srvs.cloudkafka.com:9094,ark-02.srvs.cloudkafka.com:9094,ark-03.srvs.cloudkafka.com:9094" },
            { "sasl.jaas.config", JAAS_CONFIG}
        };

        private DatabaseContext db = new DatabaseContext();

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Task launched.");
            Console.WriteLine("JAAS CONF: " + JAAS_CONFIG);
            var usersCount = db.Users.Count();
            var transactionsCount = db.Transactions.Count();
            new Producer<Null, int>(kafkaConfig, null, new IntSerializer());

            using (var producer = new Producer<Null, int>(kafkaConfig, null, new IntSerializer()))
            {
                Console.WriteLine("Attempting to write data: users - " + usersCount + ", transactions - " + transactionsCount);
                var usersResult = producer.ProduceAsync(USERS_TOPIC, null, usersCount).Result;
                var transactionsResult = producer.ProduceAsync(TRANSACTIONS_TOPIC, null, transactionsCount).Result;
                Console.WriteLine("Got data: users - " + usersResult + ", transactions - " + transactionsResult);
                await Console.Out.WriteLineAsync("Got data: users - " + usersResult + ", transactions - " + transactionsResult);
            }
        }
    }
}
