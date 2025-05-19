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
        public List<CharacterItem> CharacterItems { get; set; } = new();
        public Guid UserId { get; set; }
        public User User { get; set; }

        public void AssignItem(Item item, int quantity)
        {
            var existing = CharacterItems.FirstOrDefault(ci => ci.ItemId == item.Id);

            if (existing is not null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                CharacterItems.Add(new CharacterItem
                {
                    Item = item,
                    ItemId = item.Id,
                    CharacterId = this.Id,
                    Quantity = quantity
                });
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
