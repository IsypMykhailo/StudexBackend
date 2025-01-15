namespace Studex.Domain.Response;

public class TestResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public double Score { get; set; }
    public double MaxScore { get; set; }
    public QuestionResponse[]? Questions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class QuestionResponse
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public double Points { get; set; }
    public AnswerResponse[]? Answers { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AnswerResponse
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}