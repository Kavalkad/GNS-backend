using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public class UpdateWorkingHoursRequest
    {
        [Required] public string WorkingHoursId { get; set; } = string.Empty;
        [Required] public string NewDayOfWeek { get; set; } = string.Empty;
        [Required] public string NewStartHour { get; set; } = string.Empty;
        [Required] public string NewEndHour { get; set; } = string.Empty;
        [Required] public string NewIsOpen { get; set; } = string.Empty;
    }
}