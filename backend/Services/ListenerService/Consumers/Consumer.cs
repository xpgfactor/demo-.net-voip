using Confluent.Kafka;
using SignalR;

namespace ListenerService.Consumers
{
    public class Consumer : BackgroundService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly ListenerHub _listenerHub;

        public Consumer(ILogger<Consumer> logger,ListenerHub listenerHub)
        {
            _logger = logger;
            this._listenerHub = listenerHub;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                var configProducer = new ProducerConfig { BootstrapServers = "kafka:9092" };
                using var producer = new ProducerBuilder<Null, string>(configProducer).Build();

                var config = new ConsumerConfig
                {
                    GroupId = "",
                    BootstrapServers = "kafka:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    AllowAutoCreateTopics = true
                };
                using var consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe("");
                while (true)
                {
                    var response = consumer.Consume(TimeSpan.FromSeconds(1));
                    if (response is not null)
                        if (response.Message is not null && (response.Message.Value != string.Empty))
                        {
                            _listenerHub.Send(response.Message.Value);
                        }
                }
            });
        }
    }
}
