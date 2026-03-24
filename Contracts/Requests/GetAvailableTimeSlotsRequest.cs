using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GNS.Contracts.Requests
{
    public class GetAvailableTimeSlotsRequest : IParsable<GetAvailableTimeSlotsRequest>
    {
        [Required] public Guid CyberClubId { get; set; }
        [Required] public Guid GamingPlaceId { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public TimeSpan Duration { get; set; }

        public static GetAvailableTimeSlotsRequest Parse(string s, IFormatProvider? provider)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s), "Строка не может быть пустой или состоять из пробелов");
            }

            if (!TryParse(s, provider, out var result))
            {
                throw new FormatException(
                    $"Невозможно преобразовать строку '{s}' в {nameof(GetAvailableTimeSlotsRequest)}. " +
                    "Ожидаемый формат: \"CyberClubId,GamingPlaceId,Date(yyyy-MM-dd)\"");
            }

            return result;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out GetAvailableTimeSlotsRequest result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            var parts = s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (parts.Length != 4)
            {
                return false;
            }

            if (!Guid.TryParse(parts[0], out var cyberClubId) || cyberClubId == Guid.Empty)
            {
                return false;
            }

            if (!Guid.TryParse(parts[1], out var gamingPlaceId) || gamingPlaceId == Guid.Empty)
            {
                return false;
            }

            if (!DateTime.TryParseExact(
                    parts[2],
                    "yyyy-MM-dd",
                    provider ?? CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return false;
            }


            result = new GetAvailableTimeSlotsRequest
            {
                CyberClubId = cyberClubId,
                GamingPlaceId = gamingPlaceId,
                Date = date
            };

            return true;
        }
    }
}