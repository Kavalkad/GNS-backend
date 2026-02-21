using GNS.Interfaces;


namespace GNS.Services.Interfaces
{
    public interface IJwtProvider
    {
       string GenerateToken(IClaimsGeneratable entity);

    }
}