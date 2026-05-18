using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Data;
using PatientFollowUp.Api.Models;

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
    public async Task<ActionResult<List<FollowUpTask>>> GetTasks()
    {
        var tasks = await _dbContext.FollowUpTasks
            .OrderBy(task => task.DueDate)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FollowUpTask>> GetTaskById(int id)
    {
        var task = await _dbContext.FollowUpTasks.FindAsync(id);

        if (task is null)
        {
            return NotFound();
        }

        return Ok(task);
    }
}
