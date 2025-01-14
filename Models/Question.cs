namespace Studex.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public double Points { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; } = null!;
    public ICollection<Answer> Answers { get; } = new List<Answer>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}