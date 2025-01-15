using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ITestService
{
    Task<Test> CreateAsync(TestDto dto, CancellationToken ct = default);
    Task<Test?> GetByIdAsync(Guid id, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Test>> GetAllAsync(Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Test>> GetAllByLectureIdAsync(Guid lectureId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, TestDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}