namespace Infrastructure.MessageBroker.RabbitMq.Interfaces;

public interface IRabbitMqClient
{
    Task EnsureConnectionEstablished(Uri uri);
}