using System.ComponentModel.DataAnnotations;
using GNS.Data.Entities;

namespace GNS.Dto
{
    public class WorkingHoursDto
    {
        [Required] public string DayOfWeek { get; set; } = string.Empty;
        [Required] public string StartHour { get; set; } = string.Empty;
        [Required] public string EndHour { get; set; } = string.Empty;
        [Required] public string IsOpen { get; set; } = string.Empty;

        public WorkingHoursDto(WorkingHoursEntity wh)
        {
            DayOfWeek = wh.DayOfWeek.ToString();
            StartHour = wh.StartHour.ToString();
            EndHour = wh.EndHour.ToString();
            IsOpen = wh.IsOpen ? "Working" : "CyberClub wanna sleep zzz....."; 
        }
    }
}