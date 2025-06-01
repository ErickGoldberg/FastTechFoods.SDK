namespace FastTechFoods.SDK.MessageBus
{
    public interface IEventSubscriber
    {
        void Subscribe(string queueName, Func<string, Task> onMessageReceived);
    }
}