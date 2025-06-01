namespace FastTechFoods.SDK.MessageBus
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, string queueName);
    }
}