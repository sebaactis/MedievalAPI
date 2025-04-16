namespace MedievalGame.Domain.Exceptions
{
    public sealed class NotFoundException : DomainException
    {
        public NotFoundException(string resource)
            : base($"{resource} not found", 404) { }
    }
}
