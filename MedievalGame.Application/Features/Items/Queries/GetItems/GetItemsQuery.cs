using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Queries.GetItems
{
    public class GetItemsQuery : IRequest<List<ItemDto>>
    {
    }
}
