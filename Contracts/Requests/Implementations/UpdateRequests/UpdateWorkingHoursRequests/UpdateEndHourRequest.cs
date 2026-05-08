using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateWorkingHoursEndHourRequest : IWorkingHoursRequest
    {
        [Required] public Guid WorkingHoursId { get; set; } 
        [Required] public TimeOnly NewEndHour { get; set; } 

    }
}