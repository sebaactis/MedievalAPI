using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon
{
    public class DeleteWeaponHandler(IWeaponRepository weaponRepository, IMapper mapper) : IRequestHandler<DeleteWeaponCommand, WeaponDto>
    {
        public async Task<WeaponDto> Handle(DeleteWeaponCommand request, CancellationToken cancellationToken)
        {
            var weapon = await weaponRepository.GetByIdAsync(request.Id);
            if (weapon == null)
            {
                throw new NotFoundException($"Weapon with ID {request.Id} not found.");
            }
            await weaponRepository.DeleteAsync(weapon.Id);
            return mapper.Map<WeaponDto>(weapon);
        }
    }
}
