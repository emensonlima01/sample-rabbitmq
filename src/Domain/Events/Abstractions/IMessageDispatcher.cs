namespace Domain.Events.Abstractions;

public interface IMessageDispatcher
{
    Task DispatchMessage(object message, string exchangeName, string routingKey, string correlationId);
}
