using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;


namespace GNS.Contracts.Requests
{
    public record class CreateWorkingHoursRequest : ICyberClubRequest
    {
        [Required] public Guid CyberClubId { get; set; } 
        [Required] public string DayOfWeek { get; set; } = string.Empty;
        [Required] public TimeOnly StartHour { get; set; } 
        [Required] public TimeOnly EndHour { get; set; } 
        [Required] public bool IsOpen { get; set; } 

    }
}