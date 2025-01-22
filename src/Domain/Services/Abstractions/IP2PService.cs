using Common.DTOs;

namespace Domain.Services.Abstractions;

public interface IP2PService
{
    Task Generate(GenerateDto dto);
    Task Process(GenerateDto dto);
}