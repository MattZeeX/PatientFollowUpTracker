namespace PatientFollowUp.Api.DTOs;

public class FollowUpTaskResponse
{
    public int Id { get; set; }

    public string PatientReferenceCode { get; set; } = string.Empty;

    public string TaskType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly DueDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
