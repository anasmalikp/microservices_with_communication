using Confluent.Kafka;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Ecom.Services.OrderServices.kafka
{
    public abstract class KafkaConsumer : BackgroundService
    {
        protected readonly ILogger<KafkaConsumer> logger;
        protected readonly IDbConnection connection;
        public KafkaConsumer(IDbConnection connection, ILogger<KafkaConsumer> logger)
        {
            this.logger = logger;
            this.connection = connection;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _ = ConsumeAsync(GetTopic(), stoppingToken);
            });
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = $"{topic}-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    
                    await ProcessMessageAsync(consumeResult.Message.Value, stoppingToken);
                    
                }
                catch(Exception ex)
                {
                    logger.LogError(ex.Message);
                }
            }
        }
        protected abstract string GetTopic();
        protected abstract Task ProcessMessageAsync(string message, CancellationToken stoppingToken);
    }
}
