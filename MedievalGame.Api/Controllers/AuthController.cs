using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Auth.Commands.Register;
using MedievalGame.Application.Features.Auth.Dto;
using MedievalGame.Application.Features.Auth.Queries.Login;
using MedievalGame.Application.Features.Auth.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MedievalGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterUserCommand command)
        {
            var user = await mediator.Send(command);
            return ApiResponse<UserDto>.SuccessResponse(
                user,
                "User registered successfully",
                StatusCodes.Status201Created
                );
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginUserQuery command)
        {
            var user = await mediator.Send(command);
            return ApiResponse<AuthResponse>.SuccessResponse(
                user,
                "User login successfully",
                StatusCodes.Status200OK
               );
        }
    }
}
