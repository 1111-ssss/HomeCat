using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    [MaxLength(20)]
    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;
    public bool IsAdmin { get; set; } = false;
}