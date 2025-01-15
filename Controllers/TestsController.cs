using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Studex.Domain.Dto;
using Studex.Domain.Mappers;
using Studex.Domain.Models;
using Studex.Domain.Response;
using Studex.Services;

namespace Studex.Controllers;

[Route("api/lectures/{lectureId}/[controller]")]
[ApiController]
public class TestsController(ITestService testService) : Controller
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<TestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAll(string lectureId, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var tests = await testService.GetAllByLectureIdAsync(Guid.Parse(lectureId), userId, ct: ct);
        return Ok(TestMapper.ToResponses(tests));
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(TestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string id, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var includeProperties = new Expression<Func<Test, object>>[]
        {
            t => t.Questions,
            t => t.Questions.Select(q => q.Answers)
        };
        var test = await testService.GetByIdAsync(Guid.Parse(id), userId, includeProperties, ct);
        if (test is null)
        {
            return NotFound();
        }

        return Ok(TestMapper.ToResponse(test));
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(TestResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Create(TestDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var test = await testService.CreateAsync(userId, dto, ct: ct);
        return Created(Request.Path.Value!, TestMapper.ToResponse(test));
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, TestDto dto, CancellationToken ct)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            return Forbid();
        }
        var updated = await testService.UpdateAsync(Guid.Parse(id), userId, dto, ct);
        return updated ? Ok() : NotFound($"Test by id {id} not found");
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
        var deleted = await testService.DeleteAsync(Guid.Parse(id), userId, ct);
        return deleted ? Ok() : NotFound($"Test by id {id} not found");
    }
}