using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Commands.CreateWeapon
{
    public class CreateWeaponHandler(IWeaponRepository weaponRepository, IMapper mapper, IMediator mediator) : IRequestHandler<CreateWeaponCommand, WeaponDto>
    {
        public async Task<WeaponDto> Handle(CreateWeaponCommand request, CancellationToken ct)
        {
            try
            {
                var validator = new CreateWeaponValidator();
                await validator.ValidateAndThrowAsync(request, ct);

                var weapon = new Weapon
                {
                    Name = request.Name,
                    AttackPower = request.AttackPower,
                    Durability = request.Durability,
                    WeaponTypeId = request.WeaponTypeId,
                    RarityId = request.RarityId
                };

                var createdWeapon = await weaponRepository.AddAsync(weapon);
                var weaponDto = mapper.Map<WeaponDto>(createdWeapon);

                await mediator.Publish(new CreateWeaponNotification(weaponDto), ct);

                return weaponDto;
            }

            catch (ValidationException ex)
            {
                throw new ValidationsException(
                ex.Errors.Select(e => e.ErrorMessage));
            }
        }
    }
}
