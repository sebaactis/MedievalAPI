namespace MedievalGame.Domain.Entities
{
    public class UserAuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
