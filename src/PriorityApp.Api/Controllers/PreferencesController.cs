using Microsoft.AspNetCore.Mvc;
using PriorityApp.PriorityApp.Api.Stores;
using PriorityApp.PriorityApp.Shared;

namespace PriorityApp.PriorityApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferencesController : ControllerBase
{
    private readonly IPreferencesStore _store;
    public PreferencesController(IPreferencesStore store) => _store = store;

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserPreferences>> Get(Guid userId)
    {
        var pref = await _store.GetAsync(userId);
        return pref is null ? NotFound() : Ok(pref);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, UserPreferences dto)
    {
        if (dto.CareerWeight + dto.LifeWeight != 1m)
            return BadRequest("CareerWeight + LifeWeight must equal 1.");

        dto.UserId = userId;               // гарантуємо правильний Id
        await _store.SetAsync(dto);
        return NoContent();
    }
}