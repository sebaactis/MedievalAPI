using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalGame.Domain.Entities
{
    public class CharacterClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Ej: "Mage", "Warrior"
        public string? Description { get; set; }

        public List<Character> Characters { get; set; } = new();
    }
}
