namespace PatientFollowUp.Api.Models;

public class FollowUpTask
{
    public int Id { get; set; }

    public string PatientReferenceCode { get; set; } = string.Empty;

    public TaskType TaskType { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateOnly DueDate { get; set; }

    public FollowUpStatus Status { get; set; } = FollowUpStatus.Open;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
