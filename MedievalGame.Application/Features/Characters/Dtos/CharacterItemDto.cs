namespace MedievalGame.Application.Features.Characters.Dtos
{
    public class CharacterItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
