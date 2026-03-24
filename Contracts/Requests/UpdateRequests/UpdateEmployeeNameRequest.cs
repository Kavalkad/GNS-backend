using System.ComponentModel.DataAnnotations;
using GNS.Contracts.Requests.Interfaces;

namespace GNS.Contracts.Requests
{
    public class UpdateEmployeeNameRequest : UpdateEmployeeRequest, INameRequest
    {
        [Required] public string NewNameValue { get; set; } = string.Empty;
    }
}