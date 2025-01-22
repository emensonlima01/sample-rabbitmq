using Common.Abstractions;
using Domain.Events.Abstractions;

namespace P2P.Process.Worker
{
    public class Worker(IMessageConsumer consumer, IEventHandler handler) : BackgroundService
    {
        private readonly IMessageConsumer _consumer = consumer;
        private readonly IEventHandler _handler = handler;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           await _consumer.Consume(_handler, "p2p.events", "p2p.created", "p2p.exchange");
        }
    }
}
