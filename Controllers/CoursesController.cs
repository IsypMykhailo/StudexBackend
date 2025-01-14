using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studex.Models;

namespace Studex.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(StudexContext context) : Controller
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
    {
        if(context.Courses == null)
        {
            return NotFound();
        }
        return await context.Courses.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(string id)
    {
        if (context.Courses is null)
        {
            return NotFound();
        }
        var course = await context.Courses.FindAsync(id);
        if(course is null)
        {
            return NotFound();
        }
        return course;
    }
    
    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(Course course)
    {
        context.Courses.Add(course);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
    }
}