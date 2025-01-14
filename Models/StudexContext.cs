using Microsoft.EntityFrameworkCore;

namespace Studex.Models;

public class StudexContext(DbContextOptions<StudexContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}