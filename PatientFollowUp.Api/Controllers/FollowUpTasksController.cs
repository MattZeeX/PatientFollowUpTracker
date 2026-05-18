using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Data;
using PatientFollowUp.Api.Models;
using PatientFollowUp.Api.DTOs;

namespace PatientFollowUp.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class FollowUpTasksController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public FollowUpTasksController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static FollowUpTaskResponse ToResponse(FollowUpTask task)
    {
        return new FollowUpTaskResponse
        {
            Id = task.Id,
            PatientReferenceCode = task.PatientReferenceCode,
            TaskType = task.TaskType.ToString(),
            Description = task.Description,
            DueDate = task.DueDate,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
        };
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FollowUpTaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FollowUpTaskResponse>>> GetTasks()
    {
        var tasks = await _dbContext.FollowUpTasks
            .OrderBy(task => task.DueDate)
            .ThenBy(task => task.Id)
            .ToListAsync();

        // For this small demo API, mapping in memory keeps DTO conversion easy to read.
        // A larger API could use EF projection expressions to select fewer columns.
        var responses = tasks
            .Select(ToResponse)
            .ToList();

        return Ok(responses);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FollowUpTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FollowUpTaskResponse>> GetTaskById(int id)
    {
        var task = await _dbContext.FollowUpTasks.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        var response = ToResponse(task);

        return Ok(response);
    }

    [HttpGet("overdue")]
    [ProducesResponseType(typeof(List<FollowUpTaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FollowUpTaskResponse>>> GetOverdueTasks()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var tasks = await _dbContext.FollowUpTasks
            .Where(task =>
                task.DueDate < today &&
                task.Status != FollowUpStatus.Completed &&
                task.Status != FollowUpStatus.Cancelled)
            .OrderBy(task => task.DueDate)
            .ThenBy(task => task.Id)
            .ToListAsync();

        // For this small demo API, mapping in memory keeps DTO conversion easy to read.
        // A larger API could use EF projection expressions to select fewer columns.
        var responses = tasks
            .Select(ToResponse)
            .ToList();

        return Ok(responses);
    }

    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FollowUpTaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FollowUpTaskResponse>> CreateTask(CreateFollowUpTaskRequest request)
    {
        if (!Enum.TryParse<TaskType>(request.TaskType, ignoreCase: true, out var taskType))
        {
            return BadRequest("Invalid task type.");
        }

        var now = DateTime.UtcNow;

        var task = new FollowUpTask
        {
            PatientReferenceCode = request.PatientReferenceCode,
            TaskType = taskType,
            Description = request.Description,
            DueDate = request.DueDate,
            Status = FollowUpStatus.Open,
            CreatedAt = now,
            UpdatedAt = now,
        };

        _dbContext.FollowUpTasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var auditEvent = new AuditEvent
        {
            EntityType = nameof(FollowUpTask),
            EntityId = task.Id,
            Action = "Created",
            Timestamp = now,
            Details = "Follow-up task created.",
        };

        _dbContext.AuditEvents.Add(auditEvent);
        await _dbContext.SaveChangesAsync();

        var response = ToResponse(task);

        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, response);
    }

    [HttpPatch("{id:int}/status")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FollowUpTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FollowUpTaskResponse>> UpdateTaskStatus(int id, UpdateTaskStatusRequest request)
    {
        if (!Enum.TryParse<FollowUpStatus>(request.Status, ignoreCase: true, out var status))
        {
            return BadRequest("Invalid task status.");
        }

        var task = await _dbContext.FollowUpTasks.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        var previousStatus = task.Status;
        var now = DateTime.UtcNow;

        task.Status = status;
        task.UpdatedAt = now;

        var auditEvent = new AuditEvent
        {
            EntityType = nameof(FollowUpTask),
            EntityId = task.Id,
            Action = "StatusChanged",
            Timestamp = now,
            Details = $"Status changed from {previousStatus} to {status}.",
        };

        _dbContext.AuditEvents.Add(auditEvent);
        await _dbContext.SaveChangesAsync();

        var response = ToResponse(task);

        return Ok(response);
    }
}
