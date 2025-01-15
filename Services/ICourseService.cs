using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ICourseService
{
    Task<Course> CreateAsync(CourseDto dto, CancellationToken ct = default);
    Task<Course?> GetByIdAsync(Guid id, Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Course>> GetAllAsync(Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, CourseDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}