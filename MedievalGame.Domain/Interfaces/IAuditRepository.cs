namespace MedievalGame.Domain.Interfaces
{
    public interface IAuditRepository<T>
    {
        Task SaveAsync(T log, CancellationToken cancellationToken);
    }
}
