namespace GNS.Data.Entities
{
    public class GameEntity : BaseEntity
    {
  
        public string Title { get; set; } = null!;


        public ICollection<GamingPlaceEntity> GamingPlaces { get; set; } = new List<GamingPlaceEntity>();
        
    }
}