using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Enums;

namespace MedievalGame.Application.Features.Items.Dtos
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
    }
}
