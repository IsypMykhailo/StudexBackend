namespace Studex.Models;

public class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Topic { get; set; }
    public double Score { get; set; }
    public double MaxScore { get; set; }
    public required string Area { get; set; }
    public ICollection<Lecture> Lectures { get; } = new List<Lecture>();
    public ICollection<Test> Tests { get; } = new List<Test>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}