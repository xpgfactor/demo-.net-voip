using Confluent.Kafka;
using Kafka.Producers;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace SignalR
{
    public class StreamHub : Hub
    {
        private readonly IKafkaProducer<Null, string> _producer;

        public StreamHub(IKafkaProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public ChannelReader<int> Counter(
            int count,
            int delay,
            CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<int>();

            _ = WriteItemsAsync(channel.Writer, count, delay, cancellationToken);

            return channel.Reader;
        }

        private async Task WriteItemsAsync(
            ChannelWriter<int> writer,
            int count,
            int delay,
            CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                for (var i = 0; i < count; i++)
                {
                    await writer.WriteAsync(i, cancellationToken);

                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                localException = ex;
            }
            finally
            {
                writer.Complete(localException);
            }
        }

        public async Task UploadStream(ChannelReader<string> stream)
        {
            while (await stream.WaitToReadAsync())
            {
                while (stream.TryRead(out var item))
                {
                    _producer.ProduceAsync(null,item);
                }
            }
        }

        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("", message);
        }
    }
}
