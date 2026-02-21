using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public record class AddCyberClubRequest
    {
        [Required] public string Name { get; set; } = null!;
        [Required] public string City { get; set; } = null!;
        [Required] public string Address { get; set; } = null!;
    }
}