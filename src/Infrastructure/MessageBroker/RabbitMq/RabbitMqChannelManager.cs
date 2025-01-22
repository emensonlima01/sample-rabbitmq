using Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Infrastructure.MessageBroker.RabbitMq;

public sealed class RabbitMqChannelManager(IRabbitMqConnectionManager connectionManager) : IRabbitMqChannelManager
{
    private readonly IRabbitMqConnectionManager _connectionManager = connectionManager;
    private IConnection? _connection;
    private static readonly ConcurrentDictionary<int, IChannel> _channels = new();

    public async Task<IChannel> CreateAsync(CreateChannelOptions? channelOptions = null)
    {
        try
        {
            _connection = _connectionManager.Get();

            var channel = await _connection!.CreateChannelAsync(channelOptions).ConfigureAwait(false);

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
            if (_channels.TryRemove(channelNumber, out var channel))
            {
                if (channel.IsOpen)
                {
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
