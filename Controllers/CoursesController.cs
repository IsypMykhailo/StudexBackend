using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studex.Domain.Dto;
using Studex.Domain.Mappers;
using Studex.Domain.Response;
using Studex.Services;

namespace Studex.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(ICourseService courseService) : Controller
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<CourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll(CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var courses = await courseService.GetAllAsync(userId, ct: ct);
        return Ok(CourseMapper.ToResponses(courses));
    }
    
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var course = await courseService.GetByIdAsync(Guid.Parse(id), userId, ct: ct);
        if (course is null)
        {
            return NotFound();
        }
        return Ok(CourseMapper.ToResponse(course));
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(CourseDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var course = await courseService.CreateAsync(userId, dto, ct: ct);
        return Created(Request.Path.Value!, CourseMapper.ToResponse(course));
    }
    
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, CourseDto courseDto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var updated = await courseService.UpdateAsync(Guid.Parse(id), userId, courseDto, ct);
        return updated ? Ok() : NotFound($"Course by id {id} not found");
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var deleted = await courseService.DeleteAsync(Guid.Parse(id), userId, ct);
        return deleted ? Ok() : NotFound($"Course by id {id} not found");
    }
}