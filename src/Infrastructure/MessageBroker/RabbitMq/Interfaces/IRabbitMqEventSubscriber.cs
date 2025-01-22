using Common.Abstractions;

namespace Infrastructure.MessageBroker.RabbitMq.Interfaces;

public interface IRabbitMqEventSubscriber
{
    Task Subscribe(IEventHandler eventHandler, string queueName, string routingKey, string exchangeName, string exchangeType = "fanout");
}
