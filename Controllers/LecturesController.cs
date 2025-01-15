using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studex.Domain;
using Studex.Domain.Dto;
using Studex.Domain.Mappers;
using Studex.Domain.Response;
using Studex.Services;

namespace Studex.Controllers;

[Route("api/courses/{courseId}/[controller]")]
[ApiController]
public class LecturesController(ILectureService lectureService) : Controller
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<LectureResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll(string courseId, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var lectures = await lectureService.GetAllByCourseIdAsync(Guid.Parse(courseId), userId, ct: ct);
        return Ok(LectureMapper.ToResponses(lectures));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(LectureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var lecture = await lectureService.GetByIdAsync(Guid.Parse(id), userId, ct: ct);
        if (lecture is null)
        {
            return NotFound();
        }

        return Ok(LectureMapper.ToResponse(lecture));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(LectureResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(LectureDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var lecture = await lectureService.CreateAsync(userId, dto, ct: ct);
        return Created(Request.Path.Value!, LectureMapper.ToResponse(lecture));
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, LectureDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var updated = await lectureService.UpdateAsync(Guid.Parse(id), userId, dto, ct);
        return updated ? Ok() : NotFound($"Lecture by id {id} not found");
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
        var deleted = await lectureService.DeleteAsync(Guid.Parse(id), userId, ct);
        return deleted ? Ok() : NotFound($"Lecture by id {id} not found");
    }
}