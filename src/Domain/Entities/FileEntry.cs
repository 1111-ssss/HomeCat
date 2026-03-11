using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class FileEntry
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string FileName { get; set; } = default!;
    [MaxLength(255)]
    public string ContentType { get; set; } = default!;
    [MaxLength(255)]
    public string Path { get; set; } = default!;

    public User UploadedBy { get; set; } = default!;
}