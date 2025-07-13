using Microsoft.AspNetCore.Mvc;
using ToDoList.Models.Doings;
using ToDoList.Models.Enums;
using ToDoList.Services.Doings;

namespace ToDoList.Controllers.Doings;

[ApiController]
[Route("[controller]")]
public class DoingsController : ControllerBase
{
    private readonly IDoingsService _doingsService;

    public DoingsController(IDoingsService doingsService)
    {
        _doingsService = doingsService;
    }

    [HttpGet]
    public IEnumerable<DoingResponseDto> Get()
    {
        var doings = _doingsService.GetAll();
        return doings.Select(d => new DoingResponseDto
        {
            Id = d.Id,
            Name = d.Name,
            State = d.State.ToString(),
            Summary = d.Summary
        });
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var doing = _doingsService.GetById(id);
        if (doing == null)
            return NotFound();

        var dto = new DoingResponseDto
        {
            Id = doing.Id,
            Name = doing.Name,
            State = doing.State.ToString(),
            Summary = doing.Summary
        };

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Post([FromBody] DoingRequestDto newDoing)
    {
        if (newDoing == null)
            return BadRequest();

        var created = _doingsService.Create(newDoing.Name, newDoing.State);
        var responseDto = new DoingResponseDto
        {
            Id = created.Id,
            Name = created.Name,
            State = created.State.ToString(),
            Summary = created.Summary
        };
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, responseDto);
    }

    [HttpPatch("{id:int}")]
    public IActionResult Patch(int id, [FromBody] DoingRequestDto? updatedDto)
    {
        if (updatedDto == null)
            return BadRequest();

        var result = _doingsService.Update(id, updatedDto);

        if (result == UpdateResult.NotFound)
            return NotFound();

        if (result == UpdateResult.InvalidState)
            return BadRequest("Estado inválido.");

        var updated = _doingsService.GetById(id);
        if (updated == null)
            return NotFound(); // fallback

        var responseDto = new DoingResponseDto
        {
            Id = updated.Id,
            Name = updated.Name,
            State = updated.State.ToString(),
            Summary = updated.Summary
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var result = _doingsService.Delete(id);

        if (result == UpdateResult.NotFound)
            return NotFound();

        return NoContent();
    }
}