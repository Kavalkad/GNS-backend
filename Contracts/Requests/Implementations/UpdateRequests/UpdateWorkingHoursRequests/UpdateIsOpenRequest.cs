using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateWorkingHoursIsOpenRequest : IWorkingHoursRequest
    {
        [Required] public Guid WorkingHoursId { get; set; } 
        [Required] public bool NewIsOpen { get; set; } 
    }
}