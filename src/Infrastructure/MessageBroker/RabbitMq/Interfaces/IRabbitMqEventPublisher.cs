namespace Infrastructure.MessageBroker.RabbitMq.Interfaces;

public interface IRabbitMqEventPublisher
{
    Task PublishAsync(string message, string exchangeName, string routingKey, string correlationId);
}
