
using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public class CreateCharacterHandler(ICharacterRepository characterRepository, IMapper mapper) : IRequestHandler<CreateCharacterCommand, CharacterDto>
    {
        public async Task<CharacterDto> Handle(CreateCharacterCommand request, CancellationToken ct)
        {
            try
            {

                var validator = new CreateCharacterValidator();
                await validator.ValidateAndThrowAsync(request, ct);

                var character = new Character
                {
                    Name = request.Name,
                    Life = request.Life,
                    Attack = request.Attack,
                    Defense = request.Defense,
                    CharacterClassId = request.CharacterClassId,
                    Level = request.Level
                };

                await characterRepository.AddAsync(character);

                return mapper.Map<CharacterDto>(character);

            }
            catch (ValidationException ex)
            {
                throw new ValidationsException(
                ex.Errors.Select(e => e.ErrorMessage));
            }
        }
    }
}
