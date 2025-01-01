namespace Infrastructure.Broker.Providers.RabbitMq;

public sealed class RabbitMqConnectionManager : IRabbitMqConnectionManager
{
    private IConnection? _connection;
    private static readonly string AppName = AppDomain.CurrentDomain.FriendlyName.ToString();
    private static readonly string MachineName = Environment.MachineName.ToString();

    public async Task<IConnection> CreateAsync(Uri uri)
    {
        try
        {
            var connectionFactory = new ConnectionFactory
            {
                Uri = uri,
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