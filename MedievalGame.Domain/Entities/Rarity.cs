namespace MedievalGame.Domain.Entities
{
    public class Rarity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Weapon> Weapons { get; set; } = new();
        public List<Item> Items { get; set; } = new();
    }
}
