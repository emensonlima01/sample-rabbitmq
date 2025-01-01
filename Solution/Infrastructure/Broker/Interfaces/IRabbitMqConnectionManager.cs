namespace Infrastructure.Broker.Interfaces;

public interface IRabbitMqConnectionManager
{
    Task<IConnection> CreateAsync();

    Task CloseAsync();
}
