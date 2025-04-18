namespace MedievalGame.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public Guid ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
        public Guid RarityId { get; set; }
        public Rarity Rarity { get; set; } = null!;
        public List<Character> Characters { get; set; } = new();
    }
}
