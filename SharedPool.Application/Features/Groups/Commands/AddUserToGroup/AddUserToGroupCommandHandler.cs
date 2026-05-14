using MediatR;
using SharedPool.Application.Exceptions;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Interfaces;

namespace SharedPool.Application.Features.Groups.Commands.AddUserToGroup
{
    public class AddUserToGroupCommandHandler : IRequestHandler<AddUserToGroupCommand, Unit>
    {
        private readonly IGenericRepository<Group> _groupRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserGroup> _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddUserToGroupCommandHandler(
            IGenericRepository<Group> groupRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<UserGroup> userGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddUserToGroupCommand request, CancellationToken cancellationToken)
        {
            // Group var mı kontrolü
            var group = await _groupRepository.GetByIdAsync(request.GroupId);
            if (group == null)
                throw new BusinessException("Belirtilen grup bulunamadı");

            // Kullanıcı var mı kontrolü
            var user = await _userGroupRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new BusinessException("Belirtilen kullanıcı bulunamadı.");

            // Kullanıcı zaten bu grupta mı kontrolü
            var existingMembership = await _userGroupRepository.GetAsync(
                ug => ug.GroupId == request.GroupId && ug.UserId == request.UserId);

            if (existingMembership.Any())
                throw new BusinessException("Bu kullanıcı zaten gruba üye");
        }
    }
}
