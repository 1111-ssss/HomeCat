using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [MaxLength(16)]
    public Guid FileUrl { get; set; } = default!;

    [ForeignKey(nameof(UploadedBy))]
    public int UploadedById { get; set; }
    public User UploadedBy { get; set; } = default!;
}