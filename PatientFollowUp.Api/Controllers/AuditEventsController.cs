using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientFollowUp.Api.Data;
using PatientFollowUp.Api.DTOs;

namespace PatientFollowUp.Api.Controllers;

[ApiController]
[Route("api/audit-events")]
public class AuditEventsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public AuditEventsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AuditEventResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditEventResponse>>> GetAuditEvents()
    {
        var auditEvents = await _dbContext.AuditEvents
            .OrderByDescending(auditEvent => auditEvent.Timestamp)
            .ThenByDescending(auditEvent => auditEvent.Id)
            .Select(auditEvent => new AuditEventResponse
            {
                Id = auditEvent.Id,
                EntityType = auditEvent.EntityType,
                EntityId = auditEvent.EntityId,
                Action = auditEvent.Action,
                Timestamp = auditEvent.Timestamp,
                Details = auditEvent.Details,
            })
            .ToListAsync();

        return Ok(auditEvents);
    }
}
