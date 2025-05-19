
using BCrypt.Net;
using MedievalGame.Application.Interfaces;

namespace MedievalGame.Infraestructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
