namespace Studex.Domain.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public double Points { get; set; }
    public Guid UserAnswerId { get; set; }
    public Guid TestId { get; set; }
    public Test Test { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public Question()
    {
        Answers = new HashSet<Answer>();
    }
}