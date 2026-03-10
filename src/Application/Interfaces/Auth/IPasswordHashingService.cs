using Domain.Model.Result;

namespace Application.Interfaces.Auth;

public interface IPasswordHashingService
{
    Result<string> HashPassword(string password);
    Result VerifyPassword(string password, string hash);
}