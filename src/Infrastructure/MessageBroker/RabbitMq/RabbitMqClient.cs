using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.MessageBroker.RabbitMq;

public sealed class RabbitMqClient(IRabbitMqConnectionManager connectionManager, IConfiguration configuration) : IHostedService
{
    private readonly IRabbitMqConnectionManager _connectionManager = connectionManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var uri = _configuration["RabbitMq:Uri"];
            await _connectionManager.CreateAsync(new Uri(uri!)).ConfigureAwait(false);
        }
        catch(Exception)
        {
            throw;
        }    
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _connectionManager.CloseAsync().ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
