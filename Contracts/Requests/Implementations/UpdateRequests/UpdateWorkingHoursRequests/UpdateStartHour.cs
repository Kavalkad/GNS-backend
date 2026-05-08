using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateWorkingHoursStartHourRequest : IWorkingHoursRequest
    {
        [Required] public Guid WorkingHoursId { get; set; } 
        [Required] public TimeOnly NewStartHour { get; set; } 

    }
}