namespace MedievalGame.Domain.Entities
{
    public class Character
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Life { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Level { get; set; }
        public Guid CharacterClassId { get; set; }
        public CharacterClass CharacterClass { get; set; } = null!;
        public List<Weapon> Weapons { get; set; }
        public List<Item> Items { get; set; }

        public void AssignItem(Item item)
        {
            if (!Items.Any(i => i.Id == item.Id))
            {
                Items.Add(item);
            }
        }

        public void AssignWeapon(Weapon weapon)
        {
            if (!Weapons.Any(w => w.Id == weapon.Id))
            {
                Weapons.Add(weapon);
            }
        }
    }
}
