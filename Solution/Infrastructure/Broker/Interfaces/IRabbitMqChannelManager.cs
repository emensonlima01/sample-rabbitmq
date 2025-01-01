namespace Infrastructure.Broker.Interfaces;

public interface IRabbitMqChannelManager
{
    Task<IChannel> CreateAsync(CreateChannelOptions createChannelOptions, Uri uri);

    Task CloseAsync(int channelNumber);
}
