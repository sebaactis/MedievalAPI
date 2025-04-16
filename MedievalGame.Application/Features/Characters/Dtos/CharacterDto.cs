using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;

namespace MedievalGame.Application.Features.Characters.Dtos
{
    public class CharacterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Life { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public string Class { get; set; } 
        public List<WeaponDto>? Weapons { get; set; }
        public List<ItemDto> Items { get; set; }
    }
}
