using System.Net;

namespace MedievalGame.Domain.Exceptions
{
    public class InternalException : DomainException
    {
        public InternalException(string message = "Internal server error") : base(message, 500) { }
    }
}
