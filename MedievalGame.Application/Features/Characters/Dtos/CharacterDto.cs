namespace MedievalGame.Application.Features.Characters.Dtos
{
    public class CharacterDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Life { get; set; }
        public int Attack { get; set; }
    }
}
