namespace Domain.Model.Result;

public interface IResultBase
{
    bool IsSuccess { get; }
    ErrorCode? Error { get; }
    string? Message { get; }
    Dictionary<string, string>? Details { get; }
}