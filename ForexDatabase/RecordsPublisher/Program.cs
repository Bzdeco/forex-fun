﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private static readonly string WALLETS_TOPIC = "wallet-log";
        private static readonly Dictionary<string, object> kafkaConfig = new Dictionary<string, object>
        {
            { "bootstrap.servers", "192.168.43.70:9092" }
        };

        private HttpClient client = new HttpClient();

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Task launched.");
            int walletsCount = 0;
            client.BaseAddress = new Uri("http://localhost:50382");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("/api/wallets");
            if (response.IsSuccessStatusCode)
            {
                var wallets = await response.Content.ReadAsAsync<List<Wallet>>();
                walletsCount = wallets.Count();
                Console.WriteLine("received size: " + walletsCount);
            } else
            {
                Console.WriteLine("error");
                return;
            }

            using (var producer = new Producer<Null, string>(kafkaConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("Attempting to write wallets data: " + walletsCount);
                var walletsResult = producer.ProduceAsync(WALLETS_TOPIC, null, walletsCount+"").Result;
                await Console.Out.WriteLineAsync("Received response: " + walletsResult);
            }
        }
    }
}
