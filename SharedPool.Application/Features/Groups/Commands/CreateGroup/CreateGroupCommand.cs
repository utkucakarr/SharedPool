using MediatR;

namespace SharedPool.Application.Features.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand(
            string Name,
            Guid CreatedByUserId,
            List<Guid> MemberIds) : IRequest<Guid>;
}
