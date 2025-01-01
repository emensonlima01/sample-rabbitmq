namespace Infrastructure.Broker.Interfaces;

public interface IRabbitMqConnectionManager
{
    Task<IConnection> CreateAsync(Uri uri);

    Task CloseAsync();
}
