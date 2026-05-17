using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Models;

namespace PatientFollowUp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FollowUpTask> FollowUpTasks { get; set; }

    public DbSet<AuditEvent> AuditEvents { get; set; }
}
