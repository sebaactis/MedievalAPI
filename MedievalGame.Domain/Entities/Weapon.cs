namespace MedievalGame.Domain.Entities
{
    public class Weapon
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AttackPower { get; set; }
        public int Durability { get; set; }
        public Guid RarityId { get; set; }
        public Rarity Rarity { get; set; }
        public Guid WeaponTypeId { get; set; }
        public WeaponType WeaponType { get; set; } = null!;
        public List<Character> Characters { get; set; } = new();
    }
}
