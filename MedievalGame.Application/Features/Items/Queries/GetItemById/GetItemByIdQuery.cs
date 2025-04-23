using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Queries.GetItem
{
    public record GetItemByIdQuery(Guid Id) : IRequest<ItemDto>
    {
    }
}
