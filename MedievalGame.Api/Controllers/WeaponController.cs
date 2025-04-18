using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MedievalGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WeaponDto>>> CreateWeapon(CreateWeaponCommand command)
        {
            var weapon = await mediator.Send(command);
            return ApiResponse<WeaponDto>.SuccessResponse(
                weapon,
                "Weapon created successfully",
                StatusCodes.Status201Created
            );
        }
    }
}
