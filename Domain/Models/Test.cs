namespace Studex.Domain.Models;

public class Test
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public double Score { get; set; }
    public double MaxScore { get; set; }
    public Guid LectureId { get; set; }
    public Lecture Lecture { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Test()
    {
        Questions = new HashSet<Question>();
    }
}