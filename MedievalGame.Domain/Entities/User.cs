﻿namespace MedievalGame.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Character> Characters { get; set; } = new();

    }
}
