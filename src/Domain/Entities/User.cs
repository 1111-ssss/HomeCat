using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(20)]
    public string Username { get; set; } = default!;
    [MaxLength(255)]
    public string PasswordHash { get; set; } = default!;
    public bool IsAdmin { get; set; } = false;

    public ICollection<FileEntry>? Files { get; set; }
}