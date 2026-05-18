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

        var patientReferenceCode = "DEMO-101";
        var description = "Review demonstration lab follow-up.";
        var dueDate = new DateOnly(2026, 5, 25);

        var request = new CreateFollowUpTaskRequest
        {
            PatientReferenceCode = patientReferenceCode,
            TaskType = "LabReview",
            Description = description,
            DueDate = dueDate,
        };

        await controller.CreateTask(request);

        Assert.Single(dbContext.FollowUpTasks);
        Assert.Single(dbContext.AuditEvents);

        var task = await dbContext.FollowUpTasks.SingleAsync();
        Assert.Equal(patientReferenceCode, task.PatientReferenceCode);
        Assert.Equal(TaskType.LabReview, task.TaskType);
        Assert.Equal(description, task.Description);
        Assert.Equal(dueDate, task.DueDate);
        Assert.Equal(FollowUpStatus.Open, task.Status);

        var auditEvent = await dbContext.AuditEvents.SingleAsync();
        Assert.Equal("Created", auditEvent.Action);
        Assert.Equal(nameof(FollowUpTask), auditEvent.EntityType);
        Assert.Equal(task.Id, auditEvent.EntityId);
    }

    [Fact]
    public async Task UpdateTaskStatus_WithValidRequest_UpdatesStatusAndCreatesAuditEvent()
    {
        using var dbContext = CreateDbContext();
        var controller = new FollowUpTasksController(dbContext);

        var patientReferenceCode = "DEMO-201";
        var description = "Coordinate demonstration referral follow-up.";
        var dueDate = new DateOnly(2026, 5, 25);
        var originalTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var task = new FollowUpTask
        {
            PatientReferenceCode = patientReferenceCode,
            TaskType = TaskType.ReferralFollowUp,
            Description = description,
            DueDate = dueDate,
            Status = FollowUpStatus.Open,
            CreatedAt = originalTimestamp,
            UpdatedAt = originalTimestamp,
        };

        dbContext.FollowUpTasks.Add(task);
        await dbContext.SaveChangesAsync();

        var request = new UpdateTaskStatusRequest
        {
            Status = "Completed",
        };

        await controller.UpdateTaskStatus(task.Id, request);

        var updatedTask = await dbContext.FollowUpTasks.SingleAsync();
        Assert.Equal(FollowUpStatus.Completed, updatedTask.Status);
        Assert.True(updatedTask.UpdatedAt > originalTimestamp);

        var auditEvent = await dbContext.AuditEvents.SingleAsync();
        Assert.Equal("StatusChanged", auditEvent.Action);
        Assert.Equal(nameof(FollowUpTask), auditEvent.EntityType);
        Assert.Equal(task.Id, auditEvent.EntityId);
        Assert.Equal("Status changed from Open to Completed.", auditEvent.Details);
    }
}
