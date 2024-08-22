using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka.Producers
{
    public class SimpleStringProducer : IKafkaProducer<Null, string>
    {
        private readonly IProducer<Null, string> _producer;

        public SimpleStringProducer()
        {
            var _config = new ProducerConfig { BootstrapServers = "kafka:9092" };
            _producer = new ProducerBuilder<Null, string>(_config).Build();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task ProduceAsync(Null key, string value)
        {
            await _producer.ProduceAsync("test-topic", new Message<Null, string> { Value = value });
        }
    }
}
