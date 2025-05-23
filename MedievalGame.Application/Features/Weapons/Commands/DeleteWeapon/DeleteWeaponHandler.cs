﻿using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon
{
    public class DeleteWeaponHandler(IWeaponRepository weaponRepository, IMapper mapper, IMediator mediator) : IRequestHandler<DeleteWeaponCommand, WeaponDto>
    {
        public async Task<WeaponDto> Handle(DeleteWeaponCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException();

            var weapon = await weaponRepository.GetByIdAsync(request.Id);
            if (weapon == null)
            {
                throw new NotFoundException($"Weapon with ID {request.Id} not found.");
            }
            var weaponDeleted = await weaponRepository.DeleteAsync(weapon.Id);
            var weaponDto = mapper.Map<WeaponDto>(weaponDeleted);

            await mediator.Publish(new DeleteWeaponNotification(weaponDto), cancellationToken);

            return weaponDto;
        }
    }
}
