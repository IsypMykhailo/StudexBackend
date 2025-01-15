using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ITestService
{
    Task<Test> CreateAsync(string userId, TestDto dto, CancellationToken ct = default);
    Task<Test?> GetByIdAsync(Guid id, string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Test>> GetAllAsync(string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Test>> GetAllByLectureIdAsync(Guid lectureId, string userId, Expression<Func<Test, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, string userId, TestDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default);
}