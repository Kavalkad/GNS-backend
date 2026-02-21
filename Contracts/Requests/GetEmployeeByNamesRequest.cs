using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using GNS.Interfaces;

namespace GNS.Contracts.Requests
{
    public record class GetEmployeeByNamesRequest : IPersonRequest // IParsable<GetEmployeeByNamesRequest>
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;

      /*  public static GetEmployeeByNamesRequest Parse(string s, IFormatProvider? provider)
        {
            if (!TryParse(s, provider, out GetEmployeeByNamesRequest request))
            {
                
            }
        }

        public static bool TryParse(
            [NotNullWhen(true)] string? s,
            IFormatProvider? provider,
            [MaybeNullWhen(false)] out GetEmployeeByNamesRequest result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            var parts = s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (parts.Length != 2)
            {
                return false;
            }

            result = new GetEmployeeByNamesRequest
            {
                FirstName = parts[0],
                LastName = parts[1]
            };
            
            return true;
        } */
    }
}