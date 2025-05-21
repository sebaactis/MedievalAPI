namespace MedievalGame.Api.Requests.Characters
{
    public class CreateCharacterRequest
    {
        public string Name { get; set; } = default!;
        public int Life { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Level { get; set; }
        public Guid CharacterClassId { get; set; }
    }
}
