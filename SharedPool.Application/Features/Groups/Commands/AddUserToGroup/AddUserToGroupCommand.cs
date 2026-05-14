using MediatR;

namespace SharedPool.Application.Features.Groups.Commands.AddUserToGroup
{
    // MediatR'da geriye bir şey dönmeyeceksek (void) IRequest<Unit> kullanırız.
    public record AddUserToGroupCommand(Guid GroupId, Guid UserId) : IRequest<Unit>;
}
