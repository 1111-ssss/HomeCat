using Application.DTOs.Auth;
using Application.Handlers.Auth.Common;
using Application.Interfaces.Auth;
using Application.Interfaces.Common;
using Application.Interfaces.Repositories;
using Domain.Model.Result;

namespace Application.Handlers.Auth.Login;

public class AuthLoginHandler : IHandler<AuthLoginRequest, JwtTokenResponse>
{
    private readonly IJwtGeneratorService _jwtGeneratorService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IUserRepository _userRepository;

    public AuthLoginHandler(
        IJwtGeneratorService jwtGeneratorService,
        IPasswordHashingService passwordHashingService,
        IUserRepository userRepository
    )
    {
        _jwtGeneratorService = jwtGeneratorService;
        _passwordHashingService = passwordHashingService;
        _userRepository = userRepository;
    }

    public async Task<Result<JwtTokenResponse>> Handle(AuthLoginRequest request, CancellationToken ct)
    {
        var hashResult = _passwordHashingService.HashPassword(request.Password);
        if (!hashResult.IsSuccess)
            return Result<JwtTokenResponse>.Failed(hashResult.Error!.Value, hashResult.Message, hashResult.Details);

        var userResult = await _userRepository.GetByUsernameAsync(request.Username);
        if (userResult == null)
            return Result<JwtTokenResponse>.Failed(ErrorCode.InvalidUsernameOrPassword, "Неверное имя пользователя или пароль");

        var token = _jwtGeneratorService.GenerateToken(new GenerateTokenDTO(
            userResult.Id,
            request.Username,
            userResult.IsAdmin
                ? "Admin"
                : "User"
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