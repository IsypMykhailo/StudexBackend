using System.Linq.Expressions;
using Studex.Domain.Dto;
using Studex.Domain.Models;

namespace Studex.Services;

public interface ILectureService
{
    Task<Lecture> CreateAsync(string userId, LectureDto dto, CancellationToken ct = default);
    Task<Lecture?> GetByIdAsync(Guid id, string userid, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Lecture>> GetAllAsync(string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<IEnumerable<Lecture>> GetAllByCourseIdAsync(Guid courseId, string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, string userId, LectureDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, string userid, CancellationToken ct = default);
}