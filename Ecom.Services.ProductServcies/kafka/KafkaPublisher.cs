using Confluent.Kafka;

namespace Ecom.Services.ProductServcies.kafka
{
    public interface IKafkaPublisher
    {
        Task ProduceAsync(string topic, Message<string, string> message);
    }
    public class KafkaPublisher:IKafkaPublisher
    {
        private readonly IProducer<string, string> _producer;
        public KafkaPublisher()
        {
            var config = new ConsumerConfig
            {
                GroupId = "product-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public Task ProduceAsync(string topic, Message<string, string> message)
        {
            return _producer.ProduceAsync(topic, message);
        }

    }
}
