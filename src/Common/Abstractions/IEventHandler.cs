namespace Common.Abstractions;

public interface IEventHandler
{
    Task HandleAsync(string message);
}
