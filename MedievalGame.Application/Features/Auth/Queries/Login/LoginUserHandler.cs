using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Auth.Dto;
using MedievalGame.Application.Features.Auth.Responses;
using MedievalGame.Application.Interfaces;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Auth.Queries.Login
{
    public class LoginUserHandler(IUserRepository userRepo, IMediator mediator, IPasswordHasher hasher, IJwtProvider jwtProvider, IMapper mapper) : IRequestHandler<LoginUserQuery, AuthResponse>
    {
        public async Task<AuthResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new LoginUserValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var user = await userRepo.GetByUsernameAsync(request.Username);
                if (user == null)
                {
                    throw new NotFoundException("User not found");
                }

                if (!hasher.Verify(request.Password, user.Password))
                {
                    throw new UnauthorizedException("Invalid credentials");
                }

                var token = jwtProvider.GenerateToken(user);
                var userDto = mapper.Map<UserDto>(user);

                await mediator.Publish(new LoginUserNotification(userDto), cancellationToken);

                return new AuthResponse
                {
                    Token = token,
                    User = userDto
                };

            }
            catch (ValidationException ex)
            {
                throw new ValidationsException(
            ex.Errors.Select(e => e.ErrorMessage));
            }

        }
    }
}
