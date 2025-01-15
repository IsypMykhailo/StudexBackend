using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ICourseService
{
    Task<Course> CreateAsync(string userId, CourseDto dto, CancellationToken ct = default);
    Task<Course?> GetByIdAsync(Guid id, string userId, Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Course>> GetAllAsync(string userId, Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, string userId, CourseDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default);
}