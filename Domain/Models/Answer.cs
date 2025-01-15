namespace Studex.Domain.Models;

public class Answer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}