using Domain.Events;
using Domain.Events.Abstractions;
using Domain.Extensions;
using Domain.Services;
using Domain.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IP2PService, P2PService>();
builder.Services.AddTransient<IMessageDispatcher, MessageDispatcher>();
builder.Services.AddRabbitMq();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();