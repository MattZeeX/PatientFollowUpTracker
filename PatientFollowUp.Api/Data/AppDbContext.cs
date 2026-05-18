using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Models;

namespace PatientFollowUp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<FollowUpTask> FollowUpTasks { get; set; }

    public DbSet<AuditEvent> AuditEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data uses fake DEMO-* reference codes only
        // We won't store PHI in this demo
        modelBuilder.Entity<FollowUpTask>().HasData(
            new FollowUpTask
            {
                Id = 1,
                PatientReferenceCode = "DEMO-001",
                TaskType = TaskType.LabReview,
                Description = "Review demonstration lab follow-up task.",
                DueDate = new DateOnly(2026, 5, 15),
                Status = FollowUpStatus.Open,
                CreatedAt = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc),
            },
            new FollowUpTask
            {
                Id = 2,
                PatientReferenceCode = "DEMO-002",
                TaskType = TaskType.AppointmentReminder,
                Description = "Send demonstration appointment reminder.",
                DueDate = new DateOnly(2026, 5, 20),
                Status = FollowUpStatus.InProgress,
                CreatedAt = new DateTime(2026, 5, 11, 15, 30, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 5, 12, 16, 0, 0, DateTimeKind.Utc),
            }
        );

        modelBuilder.Entity<AuditEvent>().HasData(
            new AuditEvent
            {
                Id = 1,
                EntityType = nameof(FollowUpTask),
                EntityId = 1,
                Action = "Created",
                Timestamp = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc),
                Details = "Initial demo follow-up task created during database seeding.",
            },
            new AuditEvent
            {
                Id = 2,
                EntityType = nameof(FollowUpTask),
                EntityId = 2,
                Action = "Created",
                Timestamp = new DateTime(2026, 5, 11, 15, 30, 0, DateTimeKind.Utc),
                Details = "Initial demo follow-up task created during database seeding.",
            }
        );
    }
}
