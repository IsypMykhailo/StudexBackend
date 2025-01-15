using Microsoft.EntityFrameworkCore;
using Studex.Domain.Models;

namespace Studex.Domain;

public class StudexContext(DbContextOptions<StudexContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}