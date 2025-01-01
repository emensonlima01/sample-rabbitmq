namespace Infrastructure.Broker.Providers.RabbitMq;

public class RabbitMqChannelManager(IRabbitMqConnectionManager rabbitMqConnectionManager) : IRabbitMqChannelManager
{
    private readonly IRabbitMqConnectionManager _connectionManager = rabbitMqConnectionManager;
    private readonly ConcurrentDictionary<int, IChannel> _channels = [];
    private IConnection? _connection;

    public async Task<IChannel> CreateAsync(CreateChannelOptions createChannelOptions, Uri uri)
    {
        try
        {
            _connection = await _connectionManager.CreateAsync(uri).ConfigureAwait(false);

            var channel = await _connection.CreateChannelAsync(createChannelOptions).ConfigureAwait(false);

            _channels.TryAdd(channel.ChannelNumber, channel);

            return channel;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CloseAsync(int channelNumber)
    {
        try
        {
            if (_connection is { IsOpen: false })
            {
                _channels.TryGetValue(channelNumber, out var channel);

                if (channel is not null)
                {
                    _channels.TryRemove(channelNumber, out _);

                    await channel.CloseAsync().ConfigureAwait(false);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
