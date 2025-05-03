using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Commands.CreateItem
{
    public class CreateItemHandler(IItemRepository repository, IMapper mapper, IMediator mediator) : IRequestHandler<CreateItemCommand, ItemDto>
    {
        public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken ct)
        {
            try
            {

                var validator = new CreateItemValidator();
                await validator.ValidateAndThrowAsync(request, ct);

                var item = new Item
                {
                    Name = request.Name,
                    Value = request.Value,
                    RarityId = request.RarityId,
                    ItemTypeId = request.ItemTypeId
                };

                await repository.AddAsync(item);
                var itemDto = mapper.Map<ItemDto>(item);

                await mediator.Publish(new CreateItemNotification(itemDto), ct);

                return itemDto;

            }
            catch (ValidationException ex)
            {
                throw new ValidationsException(
                ex.Errors.Select(e => e.ErrorMessage));
            }
        }
    }
}
