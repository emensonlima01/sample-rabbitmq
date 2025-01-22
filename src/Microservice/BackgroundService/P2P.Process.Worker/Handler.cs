using Common.Abstractions;
using Common.DTOs;
using Domain.Services.Abstractions;
using System.Text.Json;

namespace P2P.Process.Worker;

public class Handler(IP2PService service) : IEventHandler
{
    private readonly IP2PService _service = service;

    public Task HandleAsync(string message)
    {
        if (string.IsNullOrEmpty(message))
            return Task.CompletedTask;

        try
        {

            var dto = JsonSerializer.Deserialize<GenerateDto>(message);
            _service.Process(dto!);
            return Task.CompletedTask;

        }
        catch (Exception)
        {
            throw;
        }
    }
}
