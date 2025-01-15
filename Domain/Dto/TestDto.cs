using Studex.Domain.Models;

namespace Studex.Domain.Dto;

public class TestDto
{
    public string Name { get; set; }
    public int QuestionsCount { get; set; }
    public QuestionDto[]? Questions { get; set; }
}

public class QuestionDto
{
    public Guid Id { get; set; }
    public Guid UserAnswerId { get; set; }
}