namespace Application.DTOs.Auth;

public record GenerateTokenDTO(
    int UserId,
    string Username,
    string Role
);