using Application.DTOs.Auth;
using Application.Handlers.Auth.Common;
using Application.Interfaces.Auth;
using Application.Interfaces.Common;
using Domain.Model.Result;

namespace Application.Handlers.Auth.Login;

public class AuthLoginHandler : IHandler<AuthLoginRequest, JwtTokenResponse>
{
    private readonly IJwtGeneratorService _jwtGeneratorService;
    private readonly IPasswordHashingService _passwordHashingService;

    public AuthLoginHandler(
        IJwtGeneratorService jwtGeneratorService,
        IPasswordHashingService passwordHashingService
    )
    {
        _jwtGeneratorService = jwtGeneratorService;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<Result<JwtTokenResponse>> Handle(AuthLoginRequest request)
    {
        var hashResult = _passwordHashingService.HashPassword(request.Password);
        if (!hashResult.IsSuccess)
            return Result<JwtTokenResponse>.Failed(hashResult.Error!.Value, hashResult.Message, hashResult.Details);

        if (request.Username != "admin" && request.Password != "admin")
            return Result<JwtTokenResponse>.Failed(ErrorCode.InvalidUsernameOrPassword, "Неверное имя пользователя или пароль");

        //get from db
        var token = _jwtGeneratorService.GenerateToken(new GenerateTokenDTO(
            1,
            request.Username,
            "Admin"
        ));
        if (token == null)
            return Result<JwtTokenResponse>.Failed(ErrorCode.InternalServerError, "Ошибка генерации токена");

        return Result<JwtTokenResponse>.Success(
            new JwtTokenResponse(
                Token: token
            )
        );
    }
}