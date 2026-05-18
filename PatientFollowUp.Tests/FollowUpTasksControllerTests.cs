using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Controllers;
using PatientFollowUp.Api.Data;
using PatientFollowUp.Api.DTOs;
using PatientFollowUp.Api.Models;

namespace PatientFollowUp.Tests;

public class FollowUpTasksControllerTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateTask_WithValidRequest_CreatesTaskAndAuditEvent()
    {
        using var dbContext = CreateDbContext();
        var controller = new FollowUpTasksController(dbContext);

        var request = new CreateFollowUpTaskRequest
        {
            PatientReferenceCode = "DEMO-101",
            TaskType = "LabReview",
            Description = "Review demonstration lab follow-up.",
            DueDate = new DateOnly(2026, 5, 25),
        };

        await controller.CreateTask(request);

        Assert.Single(dbContext.FollowUpTasks);
        Assert.Single(dbContext.AuditEvents);

        var task = await dbContext.FollowUpTasks.SingleAsync();
        Assert.Equal("DEMO-101", task.PatientReferenceCode);
        Assert.Equal(TaskType.LabReview, task.TaskType);
        Assert.Equal("Review demonstration lab follow-up.", task.Description);
        Assert.Equal(new DateOnly(2026, 5, 25), task.DueDate);
        Assert.Equal(FollowUpStatus.Open, task.Status);

        var auditEvent = await dbContext.AuditEvents.SingleAsync();
        Assert.Equal("Created", auditEvent.Action);
        Assert.Equal(nameof(FollowUpTask), auditEvent.EntityType);
        Assert.Equal(task.Id, auditEvent.EntityId);
    }
}
