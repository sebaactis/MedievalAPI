using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Commands.AssignWeapon
{
    public class AssignWeaponToCharacterHandler(ICharacterRepository characterRepository, IWeaponRepository weaponRepository, IMapper mapper) : IRequestHandler<AssignWeaponToCharacterCommand, CharacterDto>
    {
        public async Task<CharacterDto> Handle(AssignWeaponToCharacterCommand request, CancellationToken cancellationToken)
        {
            var character = await characterRepository.GetByIdAsync(request.CharacterId)
                ?? throw new NotFoundException("Character not found");

            var weapon = await weaponRepository.GetByIdAsync(request.WeaponId)
                ?? throw new NotFoundException("Weapon not found");

            character.AssignWeapon(weapon);

            await characterRepository.UpdateAsync(character);

            return mapper.Map<CharacterDto>(character);
        }
    }
}
