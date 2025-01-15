using System.Linq.Expressions;
using Mapster;
using Studex.Domain;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class TestService(StudexContext context, ICrudRepository<Test> testRepository, ICrudRepository<Question> questionRepository, ICrudRepository<Answer> answerRepository, IAIContentGenerator aiContentGenerator) : ITestService
{
    public async Task<Test> CreateAsync(string userId, TestDto dto, CancellationToken ct = default)
    {
        var test = dto.Adapt<TestDto, Test>();
        if (test.Lecture.Course.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a test for this course");
        }
        
        await testRepository.CreateAsync(test, ct);
        await testRepository.SaveAsync(ct);
        
        string prompt = $"Generate ${dto.QuestionsCount} exam questions based on the following lecture content: {test.Lecture.Content}. " +
                        "For each question, provide multiple-choice answers with one correct answer. Do not give any other text, just questions and answers. " +
                        "Correct answer should contain '(correct)' at the end. Question should start with 'Q:' and answer should start with 'A:'. " +
                        "Each question and answer should start from the new line";
        var generatedQuestions = await aiContentGenerator.GenerateContentAsync(prompt);

        // Parse generated questions and answers
        var parsedQuestions = ParseGeneratedQuestions(generatedQuestions, test.Id);

        // Add questions and answers to the repositories
        foreach (var question in parsedQuestions)
        {
            await questionRepository.CreateAsync(question, ct);
            foreach (var answer in question.Answers)
            {
                await answerRepository.CreateAsync(answer, ct);
                question.Answers.Add(answer);
            }
            question.Points = 1;
            test.Questions.Add(question);
        }
        
        test.MaxScore = test.Questions.Sum(q => q.Points);
        
        await questionRepository.SaveAsync(ct);
        await answerRepository.SaveAsync(ct);
        await testRepository.SaveAsync(ct);
        
        await CourseService.UpdateCourseScoreAsync(test.Lecture.CourseId, context);
        
        return test;
    }
    
    private List<Question> ParseGeneratedQuestions(string generatedContent, Guid testId)
    {
        var questions = new List<Question>();

        // Split the content into lines (this depends on the structure of generatedContent)
        var lines = generatedContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Question? currentQuestion = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("Q:")) // Indicates a question
            {
                if (currentQuestion != null)
                {
                    questions.Add(currentQuestion);
                }

                currentQuestion = new Question
                {
                    Text = line.Substring(2).Trim(),
                    TestId = testId,
                    CreatedAt = DateTime.UtcNow,
                    Answers = new List<Answer>()
                };
            }
            else if (line.StartsWith("A:")) // Indicates an answer
            {
                var isCorrect = line.Contains("(correct)");
                currentQuestion?.Answers.Add(new Answer
                {
                    Text = line.Replace("(correct)", "").Trim(),
                    IsCorrect = isCorrect,
                    QuestionId = currentQuestion.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        // Add the last question if it exists
        if (currentQuestion != null)
        {
            questions.Add(currentQuestion);
        }

        return questions;
    }

    public async Task<Test?> GetByIdAsync(Guid id, string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetByIdAsync(id, t => t.Lecture.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Test>> GetAllAsync(string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetAsync(t => t.Lecture.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Test>> GetAllByLectureIdAsync(Guid lectureId, string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetAsync(t => t.LectureId == lectureId && t.Lecture.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<bool> UpdateAsync(Guid id, string userId, TestDto dto, CancellationToken ct = default)
    {
        var test = await testRepository.GetByIdAsync(id, t => t.Lecture.Course.UserId == userId, ct: ct);
        if (test is null)
        {
            return false;
        }
        
        if (dto.Questions is null)
        {
            return false;
        }

        double score = 0;
        foreach (var question in dto.Questions)
        {
            var dbAnswer = answerRepository.GetByIdAsync(question.UserAnswerId, ct: ct);
            if (dbAnswer is null)
            {
                continue;
            }

            var answer = dbAnswer.Result;
            if (answer is null)
            {
                continue;
            }

            if (!answer.IsCorrect) continue;
            var dbQuestion = questionRepository.GetByIdAsync(question.Id, ct: ct);
            if (dbQuestion is null)
            {
                continue;
            }
                
            var dbQuestionResult = dbQuestion.Result;
            if (dbQuestionResult is null)
            {
                continue;
            }

            dbQuestionResult.UserAnswerId = answer.Id;
            await questionRepository.UpdateAsync(dbQuestionResult.Id, dbQuestionResult, ct);
            score += dbQuestionResult.Points;
        }
        
        await questionRepository.SaveAsync(ct);
        
        var updatedTest = dto.Adapt<TestDto, Test>();
        updatedTest.Score = score;
        updatedTest.UpdatedAt = DateTime.UtcNow;
        
        await testRepository.UpdateAsync(updatedTest.Id, updatedTest, ct);
        await testRepository.SaveAsync(ct);
        
        await CourseService.UpdateCourseScoreAsync(test.Lecture.CourseId, context);
        
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var test = await testRepository.GetByIdAsync(id, t => t.Lecture.Course.UserId == userId, ct: ct);
        if (test is null)
        {
            return false;
        }

        await testRepository.DeleteAsync(test, ct);
        await testRepository.SaveAsync(ct);
        return true;
    }
}