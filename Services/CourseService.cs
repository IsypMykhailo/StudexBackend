using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Studex.Domain;
using Studex.Domain.Dto;
using Studex.Domain.Models;
using Studex.Repositories;

namespace Studex.Services;

public class CourseService(StudexContext context, ICrudRepository<Course> courseRepository) : ICourseService
{
    public async Task UpdateCourseScoreAsync(Guid courseId)
    {
        var course = await context.Courses
            .Include(c => c.Lectures)
            .ThenInclude(l => l.Test)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course != null)
        {
            var testScores = course.Lectures
                .Where(l => l.Test != null)
                .Select(l => l.Test!.Score)
                .ToList();
            
            var maxScores = course.Lectures
                .Where(l => l.Test != null)
                .Select(l => l.Test!.MaxScore)
                .ToList();
            course.Score = testScores.Count == 0 ? testScores.Sum() : 0;
            course.MaxScore = maxScores.Count == 0 ? maxScores.Sum() : 0;

            await context.SaveChangesAsync();
        }
    }

    public async Task<Course> CreateAsync(CourseDto dto, CancellationToken ct = default)
    {
        var course = dto.Adapt<CourseDto, Course>();
        await courseRepository.CreateAsync(course, ct);
        await courseRepository.SaveAsync(ct);
        return course;
    }

    public async Task<Course?> GetByIdAsync(Guid id, Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await courseRepository.GetByIdAsync(id, null, includeProperties, ct);
    }

    public async Task<IEnumerable<Course>> GetAllAsync(Expression<Func<Course, object>>[]? includeProperties = null, CancellationToken ct = default)
    {
        return await courseRepository.GetAsync(null, includeProperties, ct);
    }

    public async Task<bool> UpdateAsync(Guid id, CourseDto dto, CancellationToken ct = default)
    {
        var course = await courseRepository.GetByIdAsync(id, ct: ct);
        if (course is null)
        {
            return false;
        }

        var updatedCourse = dto.Adapt<CourseDto, Course>();
        updatedCourse.UpdatedAt = DateTime.Now;
        await courseRepository.UpdateAsync(updatedCourse.Id, updatedCourse, ct);
        await courseRepository.SaveAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var course = await courseRepository.GetByIdAsync(id, ct: ct);
        if (course is null)
        {
            return false;
        }

        await courseRepository.DeleteAsync(course, ct);
        await courseRepository.SaveAsync(ct);
        return true;
    }
}