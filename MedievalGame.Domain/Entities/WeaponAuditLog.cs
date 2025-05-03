namespace MedievalGame.Domain.Entities
{
    public class WeaponAuditLog
    {
        public Guid Id { get; set; }
        public Guid WeaponId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
