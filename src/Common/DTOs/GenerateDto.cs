namespace Common.DTOs;

public record GenerateDto(string FromAccount, string ToAccount, string Identifier, decimal Value);