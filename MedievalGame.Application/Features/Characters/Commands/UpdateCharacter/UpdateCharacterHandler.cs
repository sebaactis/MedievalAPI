using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.UpdateCharacter
{
    public class UpdateCharacterHandler(ICharacterRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<UpdateCharacterCommand, CharacterDto>
    {
        public async Task<CharacterDto> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateCharacterValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var character = await repository.GetByIdAsync(request.Id);

            if (character == null)
            {
                throw new NotFoundException($"Character with ID {request.Id} not found.");
            }

            var updatedCharacter = await repository.UpdateAsync(mapper.Map(request, character));

            var characterDto = mapper.Map<CharacterDto>(updatedCharacter);
            await mediator.Publish(new UpdateCharacterNotification(characterDto), cancellationToken);

            return characterDto;
        }
    }
}
