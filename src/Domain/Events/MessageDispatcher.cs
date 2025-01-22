using Domain.Events.Abstractions;
using Infrastructure.MessageBroker.RabbitMq.Interfaces;
using System.Text.Json;

namespace Domain.Events;

public class MessageDispatcher(IRabbitMqEventPublisher rabbitMq) : IMessageDispatcher
{
    private readonly IRabbitMqEventPublisher _rabbitMq = rabbitMq;

    public async Task DispatchMessage(object message, string exchangeName, string routingKey, string correlationId)
    {
        try
        {
            await _rabbitMq.PublishAsync(JsonSerializer.Serialize(message), exchangeName, routingKey, correlationId).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
