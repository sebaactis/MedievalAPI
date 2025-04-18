namespace MedievalGame.Domain.Entities
{
    public class WeaponType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<Weapon> Weapons { get; set; } = new();
    }
}

