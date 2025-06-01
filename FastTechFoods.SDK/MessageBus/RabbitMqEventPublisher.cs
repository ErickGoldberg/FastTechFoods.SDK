using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FastTechFoods.SDK.MessageBus
{
    public class RabbitMqEventPublisher(IModel channel) : IEventPublisher
    {
        public Task PublishAsync<T>(T @event, string queueName)
        {
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            return Task.CompletedTask;
        }
    }
}