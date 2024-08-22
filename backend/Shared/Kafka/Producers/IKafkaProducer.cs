namespace Kafka.Producers
{
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        Task ProduceAsync(TKey key, TValue value);
    }
}
