using Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Infrastructure.MessageBroker.RabbitMq;

public sealed class RabbitMqConnectionManager : IRabbitMqConnectionManager
{
    private IConnection? _connection;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static readonly string AppName = AppDomain.CurrentDomain.FriendlyName;
    private static readonly string MachineName = Environment.MachineName;

    public async Task<IConnection> CreateAsync(Uri uri)
    {
        try
        {
            if (_connection != null && _connection.IsOpen)
            {
                return _connection;
            }

            await _semaphore.WaitAsync();
            try
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = uri,
                        ClientProvidedName = $"{AppName} on {MachineName}",
                        RequestedConnectionTimeout = TimeSpan.FromMilliseconds(1000),
                        RequestedHeartbeat = TimeSpan.FromMilliseconds(3000),
                        AutomaticRecoveryEnabled = true,
                        TopologyRecoveryEnabled = true,
                        NetworkRecoveryInterval = TimeSpan.FromMilliseconds(1000),
                    };

                    _connection = await factory.CreateConnectionAsync().ConfigureAwait(false);

                    RegisterEventHandlers();
                }

                return _connection;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        catch (BrokerUnreachableException)
        {
            throw;
        }
        catch (SocketException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task CloseAsync()
    {
        try
        {
            if (_connection != null && _connection.IsOpen)
            {
                await _connection.CloseAsync().ConfigureAwait(false);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IConnection? Get()
    {
        try
        {
            return _connection;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void RegisterEventHandlers()
    {
        _connection!.ConnectionShutdownAsync += async (sender, args) =>
        {
            await Task.CompletedTask;
        };

        _connection!.CallbackExceptionAsync += async (sender, args) =>
        {
            await Task.CompletedTask;
        };

        _connection!.RecoverySucceededAsync += async (sender, args) =>
        {
            await Task.CompletedTask;
        };

        _connection!.ConnectionBlockedAsync += async (sender, args) =>
        {
            await Task.CompletedTask;
        };

        _connection!.ConnectionUnblockedAsync += async (sender, args) =>
        {
            await Task.CompletedTask;
        };
    }
}
