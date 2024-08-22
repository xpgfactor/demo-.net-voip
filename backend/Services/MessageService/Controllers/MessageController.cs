using Confluent.Kafka;
using Kafka.Producers;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly ILogger<MessageController> _logger;
        private readonly IKafkaProducer<Null, string> _producer;

        public MessageController(ILogger<MessageController> logger, IKafkaProducer<Null, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string message)
        {
            await _producer.ProduceAsync(null, message);
            return Ok();
        }
    }
}