using System.Linq.Expressions;
using Mapster;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class TestService(ICrudRepository<Test> testRepository) : ITestService
{
    public async Task<Test> CreateAsync(TestDto dto, CancellationToken ct = default)
    {
        // TODO: Generate Questions
        var test = dto.Adapt<TestDto, Test>();
        await testRepository.CreateAsync(test, ct);
        await testRepository.SaveAsync(ct);
        return test;
    }

    public async Task<Test?> GetByIdAsync(Guid id, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetByIdAsync(id, null, includeProperties, ct);
    }

    public async Task<IEnumerable<Test>> GetAllAsync(Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetAsync(null, includeProperties, ct);
    }

    public async Task<IEnumerable<Test>> GetAllByLectureIdAsync(Guid lectureId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await testRepository.GetAsync(t => t.LectureId == lectureId, includeProperties, ct);
    }

    public async Task<bool> UpdateAsync(Guid id, TestDto dto, CancellationToken ct = default)
    {
        // TODO: Generate Questions
        var test = await testRepository.GetByIdAsync(id, ct: ct);
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

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var test = await testRepository.GetByIdAsync(id, ct: ct);
        if (test is null)
        {
            return false;
        }

        await testRepository.DeleteAsync(test, ct);
        await testRepository.SaveAsync(ct);
        return true;
    }
}