namespace Infrastructure.MessageBroker.RabbitMq.Interfaces;

public interface IRabbitMqConnectionManager
{
    Task<IConnection> CreateAsync(Uri uri);
    Task CloseAsync();
    IConnection? Get();
}
