using GNS.Data.Entities;

namespace GNS.Dto
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public GameDto(GameEntity g)
        {
            Id = g.Id;
            Title = g.Title;
        }
    }
}