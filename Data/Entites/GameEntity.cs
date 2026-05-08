namespace GNS.Data.Entities
{
    public class GameEntity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public bool OnPc { get; set; }
        public bool OnPlayStation { get; set; }
        public bool OnXbox { get; set; }

        
    }
}