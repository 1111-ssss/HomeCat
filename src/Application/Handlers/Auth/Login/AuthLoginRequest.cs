using Application.Handlers.Auth.Common;
using Application.Interfaces.Common;
using Domain.Model.Result;

namespace Application.Handlers.Auth.Login;

public record AuthLoginRequest(
    string Username,
    string Password
) : IRequest<JwtTokenResponse>;