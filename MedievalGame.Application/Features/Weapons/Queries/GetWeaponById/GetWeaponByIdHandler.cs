using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Queries.GetWeaponById
{
    public class GetWeaponByIdHandler(IWeaponRepository weaponRepository, IMapper mapper) : IRequestHandler<GetWeaponByIdQuery, WeaponDto>
    {
        public async Task<WeaponDto> Handle(GetWeaponByIdQuery request, CancellationToken cancellationToken)
        {
            var weapon = await weaponRepository.GetByIdAsync(request.Id);

            if (weapon == null)
            {
                throw new NotFoundException($"Weapon with ID {request.Id}");
            }

            return mapper.Map<WeaponDto>(weapon);
        }
    }
}
