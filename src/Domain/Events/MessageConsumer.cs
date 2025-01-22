using Common.Abstractions;
using Domain.Events.Abstractions;
using Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Domain.Events;

public class MessageConsumer(IRabbitMqEventSubscriber rabbitMq) : IMessageConsumer
{
    private readonly IRabbitMqEventSubscriber _rabbitMq = rabbitMq;

    public async Task Consume(IEventHandler handler, string queueName, string routingKey, string exchangeName, string exchangeType = "fanout")
    {
        try
        {
            await _rabbitMq.Subscribe(handler, queueName, routingKey, exchangeName, exchangeType).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
