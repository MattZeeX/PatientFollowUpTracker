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

    [HttpGet]
    public async Task<ActionResult<List<FollowUpTaskResponse>>> GetTasks()
    {
        var tasks = await _dbContext.FollowUpTasks
            .OrderBy(task => task.DueDate)
            .Select(task => new FollowUpTaskResponse
            {
                Id = task.Id,
                PatientReferenceCode = task.PatientReferenceCode,
                TaskType = task.TaskType.ToString(),
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
            })
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FollowUpTaskResponse>> GetTaskById(int id)
    {
        var task = await _dbContext.FollowUpTasks
            .Where(task => task.Id == id)
            .Select(task => new FollowUpTaskResponse
            {
                Id = task.Id,
                PatientReferenceCode = task.PatientReferenceCode,
                TaskType = task.TaskType.ToString(),
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
            })
            .FirstOrDefaultAsync();

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task);
    }
}
