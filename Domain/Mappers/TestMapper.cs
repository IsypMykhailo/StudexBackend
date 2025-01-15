using Studex.Domain.Models;
using Studex.Domain.Response;

namespace Studex.Domain.Mappers;

public class TestMapper
{
    public static TestResponse ToResponse(Test test)
    {
        return new TestResponse
        {
            Id = test.Id,
            Name = test.Name,
            Score = test.Score,
            MaxScore = test.MaxScore,
            Questions = test.Questions.Select(QuestionMapper.ToResponse).ToArray(),
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt
        };
    }
    
    public static TestResponse ToResponseWithoutQuestions(Test test)
    {
        return new TestResponse
        {
            Id = test.Id,
            Name = test.Name,
            Score = test.Score,
            MaxScore = test.MaxScore,
            CreatedAt = test.CreatedAt,
            UpdatedAt = test.UpdatedAt
        };
    }
    
    public static IEnumerable<TestResponse> ToResponses(IEnumerable<Test> tests)
    {
        var items = tests as Test[] ?? tests.ToArray();
        var responses = new TestResponse[items.Length];
        
        for (var i = 0; i < items.Length; i++)
        {
            responses[i] = ToResponseWithoutQuestions(items[i]);
        }

        return responses;
    }
}

public class QuestionMapper
{
    public static QuestionResponse ToResponse(Question question)
    {
        return new QuestionResponse
        {
            Id = question.Id,
            Text = question.Text,
            Points = question.Points,
            Answers = question.Answers.Select(AnswerMapper.ToResponse).ToArray(),
            CreatedAt = question.CreatedAt,
            UpdatedAt = question.UpdatedAt
        };
    }
}

public class AnswerMapper
{
    public static AnswerResponse ToResponse(Answer answer)
    {
        return new AnswerResponse
        {
            Id = answer.Id,
            Text = answer.Text,
            IsCorrect = answer.IsCorrect,
            CreatedAt = answer.CreatedAt,
            UpdatedAt = answer.UpdatedAt
        };
    }
}