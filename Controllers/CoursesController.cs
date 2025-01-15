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
    [ProducesResponseType(typeof(IEnumerable<CourseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll(CancellationToken ct)
    {
        var courses = await courseService.GetAllAsync(ct: ct);
        return Ok(CourseMapper.ToResponses(courses));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string id, CancellationToken ct)
    {
        var course = await courseService.GetByIdAsync(Guid.Parse(id), ct: ct);
        if (course is null)
        {
            return NotFound();
        }
        return Ok(CourseMapper.ToResponse(course));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CourseResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(CourseDto dto, CancellationToken ct)
    {
        var course = await courseService.CreateAsync(dto, ct: ct);
        return Created(Request.Path.Value!, CourseMapper.ToResponse(course));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, CourseDto courseDto, CancellationToken ct)
    {
        var updated = await courseService.UpdateAsync(Guid.Parse(id), courseDto, ct);
        return updated ? Ok() : NotFound($"Course by id {id} not found");
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var deleted = await courseService.DeleteAsync(Guid.Parse(id), ct);
        return deleted ? Ok() : NotFound($"Course by id {id} not found");
    }
}