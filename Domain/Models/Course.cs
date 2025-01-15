namespace Studex.Domain.Models;

public class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Topic { get; set; }
    public double Score { get; set; }
    public double MaxScore { get; set; }
    public required string Area { get; set; }
    public ICollection<Lecture> Lectures { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public Course()
    {
        Lectures = new HashSet<Lecture>();
    }
}