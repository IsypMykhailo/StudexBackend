using System.Linq.Expressions;
using Mapster;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class LectureService(ICrudRepository<Lecture> lectureRepository) : ILectureService
{
    public async Task<Lecture> CreateAsync(string userid, LectureDto dto, CancellationToken ct = default)
    {
        // TODO: Generate Content
        var lecture = dto.Adapt<LectureDto, Lecture>();
        if (lecture.Course.UserId != userid)
        {
            throw new UnauthorizedAccessException("You are not authorized to create a lecture for this course");
        }
        await lectureRepository.CreateAsync(lecture, ct);
        await lectureRepository.SaveAsync(ct);
        return lecture;
    }

    public async Task<Lecture?> GetByIdAsync(Guid id, string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Lecture>> GetAllAsync(string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetAsync(l => l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<IEnumerable<Lecture>> GetAllByCourseIdAsync(Guid courseId, string userId, Expression<Func<Lecture, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await lectureRepository.GetAsync(l => l.CourseId == courseId && l.Course.UserId == userId, includeProperties, ct);
    }

    public async Task<bool> UpdateAsync(Guid id, string userId, LectureDto dto, CancellationToken ct = default)
    {
        // TODO: Generate Content
        var lecture = await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, ct: ct);
        if (lecture is null)
        {
            return false;
        }

        var updatedLecture = dto.Adapt<LectureDto, Lecture>();
        updatedLecture.UpdatedAt = DateTime.UtcNow;
        await lectureRepository.UpdateAsync(updatedLecture.Id, updatedLecture, ct);
        await lectureRepository.SaveAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var lecture = await lectureRepository.GetByIdAsync(id, l => l.Course.UserId == userId, ct: ct);
        if (lecture is null)
        {
            return false;
        }

        await lectureRepository.DeleteAsync(lecture, ct);
        await lectureRepository.SaveAsync(ct);
        return true;
    }
}