namespace Infrastructure.MessageBroker.RabbitMq.Interfaces;

public interface IRabbitMqChannelManager
{
    Task<IChannel> CreateAsync(CreateChannelOptions? channelOptions = null);
    Task CloseAsync(int channelNumber);
}
