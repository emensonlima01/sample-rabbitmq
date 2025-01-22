using Common.DTOs;
using Domain.Events.Abstractions;
using Domain.Services.Abstractions;

namespace Domain.Services;

public class P2PService(IMessageDispatcher dispatcher) : IP2PService
{
    private readonly IMessageDispatcher _dispatcher = dispatcher;

    public async Task Generate(GenerateDto dto)
    {
        try
        {
            await _dispatcher.DispatchMessage(dto, "p2p.events", "p2p.created", dto.Identifier).ConfigureAwait(false);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task Process(GenerateDto dto)
    {
        try
        {
            return Task.CompletedTask;
        }
        catch (Exception)
        {
            throw;
        }
    }
}