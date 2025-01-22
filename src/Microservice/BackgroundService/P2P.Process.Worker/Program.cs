using Common.Abstractions;
using Domain.Events;
using Domain.Events.Abstractions;
using Domain.Extensions;
using Domain.Services;
using Domain.Services.Abstractions;
using P2P.Process.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IP2PService, P2PService>();
builder.Services.AddScoped<IEventHandler, Handler>();
builder.Services.AddTransient<IMessageConsumer, MessageConsumer>();
builder.Services.AddRabbitMq();

var host = builder.Build();
await host.RunAsync();
