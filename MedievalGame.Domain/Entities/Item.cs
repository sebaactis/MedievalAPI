using MedievalGame.Domain.Enums;

namespace MedievalGame.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public ItemType Type { get; set; }
        public Rarity Rarity { get; set; }
        public List<Character> Characters { get; set; } = new();
    }

    public enum ItemType
    {
        Consumable,
        Equipable
    }
}
