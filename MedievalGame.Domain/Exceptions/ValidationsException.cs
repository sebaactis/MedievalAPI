namespace MedievalGame.Domain.Exceptions
{
    public class ValidationsException : DomainException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationsException(IEnumerable<string> errors)
            : base("Validation errors", 400)
            => Errors = errors;
    }
}
