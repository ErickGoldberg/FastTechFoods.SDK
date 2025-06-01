using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client;

namespace FastTechFoods.SDK.MessageBus
{
    public class RabbitMqEventSubscriber(IModel channel) : IEventSubscriber
    {
        public void Subscribe(string queueName, Func<string, Task> onMessageReceived)
        {
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onMessageReceived(message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}