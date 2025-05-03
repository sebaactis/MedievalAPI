namespace MedievalGame.Domain.Entities
{
    public class ItemAuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
