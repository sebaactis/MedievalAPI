namespace MedievalGame.Domain.Entities
{
    public class CharacterItem
    {
        public Guid CharacterId { get; set; }
        public Character Character { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}
