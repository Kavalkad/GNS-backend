using System.ComponentModel.DataAnnotations;
using GNS.Data.Entities;

namespace GNS.Dto
{
    public class WorkingHoursDto
    {
        [Required] public Guid Id { get; set; }
        [Required] public string DayOfWeek { get; set; } = string.Empty;
        [Required] public TimeOnly StartHour { get; set; }
        [Required] public TimeOnly EndHour { get; set; } 
        [Required] public bool IsOpen { get; set; } 
    }
}