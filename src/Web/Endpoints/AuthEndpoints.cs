using Microsoft.AspNetCore.Mvc;
using Domain.Model.Result;
using Application.Handlers.Auth.Login;
using Application.Handlers.Auth.Common;

namespace API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            // .RequireRateLimiting("StrictLimiter")
            .WithTags("Аутентификация");

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Вход в систему")
            .WithDescription("Позволяет пользователю войти в систему, предоставив имя пользователя и пароль. В случае успешной аутентификации возвращает JWT-токен для доступа к защищенным ресурсам API.")
            .Accepts<AuthLoginRequest>("application/json")
            .Produces<JwtTokenResponse>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> LoginAsync(
        [FromServices] AuthLoginHandler handler,
        [FromBody] AuthLoginRequest request
    )
    {
        var result = await handler.Handle(request);

        return result.ToApiResult();
    }
}