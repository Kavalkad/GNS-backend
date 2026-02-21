using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class CreateWorkingHoursRequest
    {
        [Required] public string CyberClubId { get; set; } = string.Empty;
        [Required] public string DayOfWeek { get; set; } = string.Empty;
        [Required] public string StartHour { get; set; } = string.Empty;
        [Required] public string EndHour { get; set; } = string.Empty;
        [Required] public string IsOpen { get; set; } = string.Empty;

    }
}