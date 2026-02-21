using GNS.Enums;
namespace GNS.Interfaces
{
    public interface IClaimsGeneratable
    {
        Guid Id { get; }
        Role Role { get;  }
    }
}