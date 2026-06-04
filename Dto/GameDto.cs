
namespace GNS.Dto
{
    public record class GameDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<string> AvailableOn { get; set; } = [];
        
    }
}