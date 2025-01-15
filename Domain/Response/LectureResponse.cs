namespace Studex.Domain.Response;

public class LectureResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = default!;
    public required string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}