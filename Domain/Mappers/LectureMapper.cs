using Studex.Domain.Models;
using Studex.Domain.Response;

namespace Studex.Domain.Mappers;

public class LectureMapper
{
    public static LectureResponse ToResponse(Lecture obj)
    {
        return new()
        {
            Id = obj.Id,
            Name = obj.Name,
            Content = obj.Content,
            CreatedAt = obj.CreatedAt,
            UpdatedAt = obj.UpdatedAt
        };
    }
    
    public static IEnumerable<LectureResponse> ToResponses(IEnumerable<Lecture> objEnumerable)
    {
        var items = objEnumerable as Lecture[] ?? objEnumerable.ToArray();
        var responses = new LectureResponse[items.Length];
        
        for (var i = 0; i < items.Length; i++)
        {
            responses[i] = ToResponse(items[i]);
        }

        return responses;
    }
}