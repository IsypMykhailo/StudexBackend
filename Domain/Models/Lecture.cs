namespace Studex.Domain.Models;

public class Lecture
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Content { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public Test? Test { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}