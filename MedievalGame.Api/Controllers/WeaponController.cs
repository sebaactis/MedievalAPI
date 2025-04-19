using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacters;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;
using MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Application.Features.Weapons.Queries.GetWeaponById;
using MedievalGame.Application.Features.Weapons.Queries.GetWeapons;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<WeaponDto>>>> GetWeapons()
        {
            var weapons = await mediator.Send(new GetWeaponsQuery());
            return ApiResponse<List<WeaponDto>>.SuccessResponse(
                weapons,
                "Weapons retrieved successfully",
                StatusCodes.Status200OK
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WeaponDto>>> GetWeaponById(Guid id)
        {
            var weapon = await mediator.Send(new GetWeaponByIdQuery(id));
            return ApiResponse<WeaponDto>.SuccessResponse(
                weapon,
                "Weapon found successfully",
                StatusCodes.Status200OK
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WeaponDto>>> UpdateWeapon(Guid id,
            [FromBody] UpdateWeaponCommand command)
        {
            if (id != command.Id)
                return ApiResponse<WeaponDto>.ErrorResponse("ID mismatch", StatusCodes.Status400BadRequest);

            var weaponUpdate = await mediator.Send(command);
            return ApiResponse<WeaponDto>.SuccessResponse(
                    weaponUpdate,
                    "Weapon updated successfully",
                    StatusCodes.Status200OK
                );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<WeaponDto>>> DeleteCharacter(Guid id)
        {
            var weaponDeleted = await mediator.Send(new DeleteWeaponCommand(id));
            return ApiResponse<WeaponDto>.SuccessResponse(
                weaponDeleted,
                "Weapon deleted successfully",
                StatusCodes.Status200OK
            );
        }
    }
}
