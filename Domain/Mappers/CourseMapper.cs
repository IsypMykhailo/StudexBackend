using Studex.Domain.Models;
using Studex.Domain.Response;

namespace Studex.Domain.Mappers;

public class CourseMapper
{
    public static CourseResponse ToResponse(Course obj)
    {
        return new CourseResponse
        {
            Id = obj.Id,
            Name = obj.Name,
            Topic = obj.Topic,
            Score = obj.Score,
            MaxScore = obj.MaxScore,
            Area = obj.Area,
            CreatedAt = obj.CreatedAt,
            UpdatedAt = obj.UpdatedAt
        };
    }

    public static IEnumerable<CourseResponse> ToResponses(IEnumerable<Course> objEnumerable)
    {
        var items = objEnumerable as Course[] ?? objEnumerable.ToArray();
        var responses = new CourseResponse[items.Length];
        
        for (var i = 0; i < items.Length; i++)
        {
            responses[i] = ToResponse(items[i]);
        }

        return responses;
    }
}