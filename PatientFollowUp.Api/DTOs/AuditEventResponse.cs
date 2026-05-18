namespace PatientFollowUp.Api.DTOs;

public class AuditEventResponse
{
    public int Id { get; set; }

    public string EntityType { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Action { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public string Details { get; set; } = string.Empty;
}
