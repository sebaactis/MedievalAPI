using AutoMapper;
using FluentValidation;
using MediatR;
using MedievalGame.Application.Features.Auth.Dto;
using MedievalGame.Application.Interfaces;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Auth.Commands.Register
{
    public class RegisterUserHandler(IUserRepository userRepo, IMediator mediator, IMapper mapper, IPasswordHasher hasher) : IRequestHandler<RegisterUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new RegisterUserValidator();
                await validator.ValidateAndThrowAsync(request, cancellationToken);

                var findUser = await userRepo.GetByUsernameAsync(request.Username);

                if (findUser != null)
                {
                    throw new DomainException($"Username: {request.Username} is already in use");
                }

                var user = new User
                {
                    Username = request.Username,
                    Password = hasher.Hash(request.Password)
                };

                await userRepo.AddAsync(user);

                var userDto = mapper.Map<UserDto>(user);

                await mediator.Publish(new RegisterUserNotification(userDto), cancellationToken);

                return userDto;
            }
            catch (ValidationException ex)
            {
                throw new ValidationsException(
                ex.Errors.Select(e => e.ErrorMessage));
            }



        }
    }
}
