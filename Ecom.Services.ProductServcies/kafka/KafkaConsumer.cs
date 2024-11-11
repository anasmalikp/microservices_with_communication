using Confluent.Kafka;
using Dapper;
using Ecom.Data.Models.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Ecom.Services.ProductServcies.kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IDbConnection connection;
        private readonly ILogger<KafkaConsumer> logger;

        public KafkaConsumer(IConfiguration config, ILogger<KafkaConsumer> logger)
        {
            connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _ = ConsumeAsync("order-topic", stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var order = JsonConvert.DeserializeObject<Orders>(consumeResult.Message.Value);
                    if (order != null)
                    {
                        await connection.ExecuteAsync("UPDATE products SET stock = stock - @qty WHERE id=@id", new { qty = order.qty, id = order.id });
                    }
                }
                catch (ConsumeException e)
                {
                    logger.LogError($"Error consuming message: {e.Error.Reason}");
                }
            }
            consumer.Close();
        }
    }
}
