
using MediatR;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public class CreateCharacterHandler : IRequestHandler<CreateCharacterCommand, Guid>
    {
        private readonly ICharacterRepository _characterRepository;

        public CreateCharacterHandler(ICharacterRepository characterRepository) => _characterRepository = characterRepository;

        public async Task<Guid> Handle(CreateCharacterCommand request, CancellationToken ct)
        {
            var character = new Character
            {
                Name = request.Name,
                Life = request.Life,
                Attack = request.Attack,
                Defense = request.Defense,
                Class = request.Class,
                Level = request.Level
            };

            await _characterRepository.AddAsync(character);
            return character.Id;
        }
    }
}
