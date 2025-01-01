using RabbitMQ.Client;

namespace Infrastructure.Broker.Providers.RabbitMq;

public sealed class RabbitMqConnectionManager(Uri uri)
{
    private IConnection? _connection;
    private readonly Uri _uri = uri;
    private static readonly string AppName = AppDomain.CurrentDomain.FriendlyName.ToString();
    private static readonly string MachineName = Environment.MachineName.ToString();

    public async Task<IConnection> CreateAsync()
    {
        try
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = _uri,
                ClientProvidedName = $"MachineName:{MachineName} - AppName:{AppName}"
            };

            _connection = await connectionFactory.CreateConnectionAsync(connectionFactory.ClientProvidedName).ConfigureAwait(false);

            return _connection!;
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task CloseAsync()
    {
        try
        {
            if (_connection is { IsOpen: true })
            {
                await _connection.CloseAsync().ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
