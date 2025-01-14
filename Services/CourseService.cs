using Microsoft.EntityFrameworkCore;
using Studex.Models;

namespace Studex.Services;

public class CourseService(StudexContext context)
{
    public async Task UpdateCourseScoreAsync(Guid courseId)
    {
        var course = await context.Courses
            .Include(c => c.Tests)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course != null)
        {
            course.Score = course.Tests.Count == 0 ? course.Tests.Sum(t => t.Score) : 0;
            course.MaxScore = course.Tests.Count == 0 ? course.Tests.Sum(t => t.MaxScore) : 0;

            await context.SaveChangesAsync();
        }
    }
}