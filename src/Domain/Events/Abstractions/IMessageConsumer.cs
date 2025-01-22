using Common.Abstractions;

namespace Domain.Events.Abstractions;

public interface IMessageConsumer
{
    Task Consume(IEventHandler handler, string queueName, string routingKey, string exchangeName, string exchangeType = "fanout");
}
