using System.ComponentModel.DataAnnotations;

namespace GNS.Contracts.Requests
{
    public class UpdateCyberClubRequest
    {
        [Required] public string Name { get; set; } = null!;
        public string? NewName { get; set; } 
        public string? NewCity { get; set; } 
        public string? NewAddress { get; set; }
    }
}