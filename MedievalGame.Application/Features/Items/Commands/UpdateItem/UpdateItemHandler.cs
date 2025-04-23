using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Commands.UpdateItem
{
    public class UpdateItemHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<UpdateItemCommand, ItemDto>
    {
        public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateItemValidator();
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var item = await repository.GetByIdAsync(request.Id);

            if (item == null)
            {
                throw new NotFoundException($"Item with ID {request.Id} not found.");
            }

            mapper.Map(request, item);

            var updatedItem = await repository.UpdateAsync(item);

            return mapper.Map<ItemDto>(updatedItem);
        }
    }
}
