using Infrastructure.MessageBroker.RabbitMq;
using Infrastructure.MessageBroker.RabbitMq.Events;
using Infrastructure.MessageBroker.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;

public static class MessageBrokerExtension
{
    public static void AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqConnectionManager, RabbitMqConnectionManager>();
        services.AddSingleton<IRabbitMqChannelManager, RabbitMqChannelManager>();
        services.AddSingleton<IRabbitMqEventSubscriber, RabbitMqEventSubscriber>();
        services.AddSingleton<IRabbitMqEventPublisher, RabbitMqEventPublisher>();

        services.AddHostedService<RabbitMqClient>();
    }
}
