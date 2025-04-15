using MedievalGame.Domain.Enums;

namespace MedievalGame.Domain.Entities
{
    public class Weapon
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AttackPower { get; set; }
        public int Durability { get; set; }
        public Rarity Rarity { get; set; }
        public WeaponType Type { get; set; }
        public Guid CharacterId { get; set; }
        public Character Character { get; set; } = null!;
    }
}
