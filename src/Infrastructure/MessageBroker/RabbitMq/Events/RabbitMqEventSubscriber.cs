namespace Infrastructure.MessageBroker.RabbitMq.Events;

public class RabbitMqEventSubscriber(IRabbitMqChannelManager channelManager) : IRabbitMqEventSubscriber
{
    private static readonly ConcurrentDictionary<int, IChannel> _channels = new();

    public async Task Subscribe(IEventHandler eventHandler, string queueName, string routingKey, string exchangeName, string exchangeType = "fanout")
    {
        try
        {
            var channel = await channelManager.CreateAsync().ConfigureAwait(false);

            _channels.TryAdd(channel.ChannelNumber, channel);

            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: exchangeType, durable: true, false).ConfigureAwait(false);

            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null).ConfigureAwait(false);

            await channel.BasicQosAsync(0, 1, false).ConfigureAwait(false);

            await channel.QueueBindAsync(queueName, exchangeName, routingKey).ConfigureAwait(false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    if (eventHandler != null)
                    {
                        await eventHandler.HandleAsync(message).ConfigureAwait(false);
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true).ConfigureAwait(false);
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
