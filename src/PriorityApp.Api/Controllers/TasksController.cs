using Microsoft.AspNetCore.Mvc;
using PriorityApp.PriorityApp.Api.Services;
using PriorityApp.PriorityApp.Api.Stores;
using PriorityApp.PriorityApp.Shared;

namespace PriorityApp.PriorityApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskStore _taskStore;
    private readonly IPreferencesStore _prefStore;
    private readonly IScoringService _scoring;

    public TasksController(ITaskStore taskStore,
                           IPreferencesStore prefStore,
                           IScoringService scoring)
    {
        _taskStore = taskStore;
        _prefStore = prefStore;
        _scoring   = scoring;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<List<TaskItem>>> GetAll(Guid userId)
        => Ok((await _taskStore.GetAllAsync(userId)).ToList());

    [HttpPost("{userId:guid}")]
    public async Task<ActionResult<TaskItem>> Create(Guid userId, TaskItem dto)
    {
        var pref = await _prefStore.GetAsync(userId)
               ?? throw new InvalidOperationException("User preferences not set.");

        dto.Id = Guid.NewGuid();
        dto.UserId = userId;
        dto.CreatedAt = DateTime.UtcNow;
        dto.UpdatedAt = DateTime.UtcNow;
        dto.ComputedScore = _scoring.CalculateScore(dto, pref);

        await _taskStore.AddAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { userId }, dto);
    }

    [HttpPut("{userId:guid}/{taskId:guid}")]
    public async Task<IActionResult> Update(Guid userId, Guid taskId, TaskItem dto)
    {
        // This would only work if you want to assign a default TaskItem instead of returning
        var existing = await _taskStore.GetAsync(userId, taskId) ?? new TaskItem();

        var pref = await _prefStore.GetAsync(userId)
                ?? throw new InvalidOperationException("Preferences missing.");

        // Оновлюємо поля (не змінюємо Id і UserId)
        existing.Title            = dto.Title;
        existing.Domain           = dto.Domain;
        existing.DueDate          = dto.DueDate;
        existing.EstimatedHours   = dto.EstimatedHours;
        existing.Pg               = dto.Pg;
        existing.Dependency       = dto.Dependency;
        existing.Impact           = dto.Impact;
        existing.WeightEffort     = dto.WeightEffort;
        existing.WeightDependency = dto.WeightDependency;
        existing.WeightImpact     = dto.WeightImpact;
        existing.WeightUrgency    = dto.WeightUrgency;
        existing.UpdatedAt        = DateTime.UtcNow;

        existing.ComputedScore = _scoring.CalculateScore(existing, pref);
        await _taskStore.UpdateAsync(existing);
        return NoContent();
    }

    [HttpDelete("{userId:guid}/{taskId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, Guid taskId)
    {
        await _taskStore.DeleteAsync(userId, taskId);
        return NoContent();
    }
}