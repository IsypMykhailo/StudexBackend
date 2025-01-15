using System.Linq.Expressions;
using Mapster;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class TestService(ICrudRepository<Test> testRepository) : ITestService
{
    public async Task<Test> CreateAsync(string userId, TestDto dto, CancellationToken ct = default)
    {
        // TODO: Generate Questions
        var test = dto.Adapt<TestDto, Test>();
        if (test.Lecture.Course.UserId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a test for this course");
        }
        await testRepository.CreateAsync(test, ct);
        await testRepository.SaveAsync(ct);
        return test;
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
        // TODO: Generate Questions
        var test = await testRepository.GetByIdAsync(id, t => t.Lecture.Course.UserId == userId, ct: ct);
        if (test is null)
        {
            return false;
        }

        var updatedTest = dto.Adapt<TestDto, Test>();
        updatedTest.UpdatedAt = DateTime.UtcNow;
        await testRepository.UpdateAsync(updatedTest.Id, updatedTest, ct);
        await testRepository.SaveAsync(ct);
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