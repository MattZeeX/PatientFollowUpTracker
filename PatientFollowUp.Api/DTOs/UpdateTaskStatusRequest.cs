using System.ComponentModel.DataAnnotations;

namespace PatientFollowUp.Api.DTOs;

public class UpdateTaskStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
