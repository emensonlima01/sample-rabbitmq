using Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Infrastructure.MessageBroker.RabbitMq.Events;

public class RabbitMqEventPublisher(IRabbitMqChannelManager channelManager) : IRabbitMqEventPublisher
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private IChannel? _channel;
    private static readonly CreateChannelOptions _channelOptions = new(
        publisherConfirmationsEnabled: true,
        publisherConfirmationTrackingEnabled: true
    );
    private static readonly BasicProperties _basicProperties = new()
    {
        DeliveryMode = DeliveryModes.Persistent,
        ContentType = "text/plain",
        ContentEncoding = "UTF-8",
        Priority = 0
    };

    public async Task PublishAsync(string message, string exchangeName, string routingKey, string correlationId)
    {
        try
        {
            if (_channel == null || _channel.IsClosed)
            {
                await _semaphore.WaitAsync();
                try
                {
                    if (_channel == null || _channel.IsClosed)
                    {
                        _channel = await channelManager.CreateAsync(_channelOptions).ConfigureAwait(false);

                        Console.WriteLine("Channel created.");
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            _basicProperties.MessageId = Guid.NewGuid().ToString();
            _basicProperties.CorrelationId = correlationId;

            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: _basicProperties,
                body: body
            ).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}