using GNS.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GNS.Services.Interfaces;


namespace GNS.Services.Implementations
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }
        
       public string GenerateToken(IClaimsGeneratable entity)
        {
            var claims = ClaimsBuilder.GenerateClaims(entity);
             var signinngCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(
                                            Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                                        SecurityAlgorithms.HmacSha256
                                    );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signinngCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiredHours)
            );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
           
        }
    
    }
}