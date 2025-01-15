using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ILectureService
{
    Task<Lecture> CreateAsync(LectureDto dto, CancellationToken ct = default);
    Task<Lecture?> GetByIdAsync(Guid id, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Lecture>> GetAllAsync(Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Lecture>> GetAllByCourseIdAsync(Guid courseId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, LectureDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}