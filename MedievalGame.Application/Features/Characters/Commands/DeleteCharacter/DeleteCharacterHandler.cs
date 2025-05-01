using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.DeleteCharacter
{
    public class DeleteCharacterHandler(ICharacterRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<DeleteCharacterCommand, CharacterDto>
    {
        public async Task<CharacterDto> Handle(DeleteCharacterCommand request, CancellationToken ct)
        {
            var character = await repository.GetByIdAsync(request.Id);

            if (character is null)
                throw new NotFoundException("Character");

            var characterDeleted = await repository.DeleteAsync(character.Id);
            var characterDto = mapper.Map<CharacterDto>(characterDeleted);

            await mediator.Publish(new DeleteCharacterNotification(characterDto), ct);

            return characterDto;

        }
    }
}
