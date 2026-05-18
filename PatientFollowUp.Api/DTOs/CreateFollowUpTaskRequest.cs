using System.ComponentModel.DataAnnotations;

namespace PatientFollowUp.Api.DTOs;

public class CreateFollowUpTaskRequest
{
    [Required]
    [RegularExpression(@"^DEMO-\d{3}$", ErrorMessage = "PatientReferenceCode must use the fake demo format DEMO-001.")]
    public string PatientReferenceCode { get; set; } = string.Empty;

    [Required]
    public string TaskType { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 5)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly DueDate { get; set; }
}
