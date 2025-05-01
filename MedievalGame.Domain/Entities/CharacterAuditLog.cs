
namespace MedievalGame.Domain.Entities
{
    public class CharacterAuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CharacterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
