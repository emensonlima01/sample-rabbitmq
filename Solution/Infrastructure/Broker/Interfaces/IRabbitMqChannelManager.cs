namespace Infrastructure.Broker.Interfaces;

public interface IRabbitMqChannelManager
{
    Task<IChannel> CreateAsync(CreateChannelOptions createChannelOptions);

    Task CloseAsync(int channelNumber);
}
