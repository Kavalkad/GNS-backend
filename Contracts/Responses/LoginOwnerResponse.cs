using System.ComponentModel.DataAnnotations;
using GNS.Dto;

namespace GNS.Contracts.Responses
{
    public record class LoginOwnerResponse : LoginUserResponse
    {
        [Required] public ICollection<CyberClubDto> CyberClubs { get; set; } = [];
    }
}