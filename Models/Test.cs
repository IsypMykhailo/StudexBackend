namespace Studex.Models;

public class Test
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public double Score { get; set; }
    public double MaxScore { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public ICollection<Question> Questions { get; } = new List<Question>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}