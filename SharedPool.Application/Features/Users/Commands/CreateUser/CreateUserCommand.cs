using MediatR;

namespace SharedPool.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(
            string FirstName,
            string LastName,
            string Email,
            string Password
        ) : IRequest<Guid>;
}
