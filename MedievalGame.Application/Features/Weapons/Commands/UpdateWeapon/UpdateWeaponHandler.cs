using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon
{
    public class UpdateWeaponHandler(IWeaponRepository weaponRepository, IMapper mapper, IMediator mediator) : IRequestHandler<UpdateWeaponCommand, WeaponDto>
    {
        public async Task<WeaponDto> Handle(UpdateWeaponCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateWeaponValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var weapon = await weaponRepository.GetByIdAsync(request.Id);

            if (weapon == null)
            {
                throw new NotFoundException($"Weapon with ID {request.Id} not found.");
            }

            mapper.Map(request, weapon);

            var updatedWeapon = await weaponRepository.UpdateAsync(weapon);
            var weaponDto = mapper.Map<WeaponDto>(updatedWeapon);

            await mediator.Publish(new UpdateWeaponNotification(weaponDto), cancellationToken);

            return weaponDto;
        }
    }
}
