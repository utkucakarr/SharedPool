using MediatR;
using SharedPool.Application.Exceptions;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SharedPool.Application.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
    {
        private readonly IGenericRepository<Group> _groupRepository;
        private readonly IGenericRepository<UserGroup> _userGroupRepository;
        private readonly IGenericRepository<User> _userRepository; // + Kullanıcı kontrolü için eklendi
        private readonly IUnitOfWork _unitOfWork;

        public CreateGroupCommandHandler(
            IGenericRepository<Group> groupRepository,
            IGenericRepository<UserGroup> userGroupRepository,
            IGenericRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            // Grubu kuran kişi de dahil olmak üzere tüm ID'leri bir listede toplayalım
            var allUserIds = new List<Guid> { request.CreatedByUserId };
            if (request.MemberIds != null && request.MemberIds.Any())
            {
                allUserIds.AddRange(request.MemberIds);
            }

            allUserIds = allUserIds.Distinct().ToList(); // Aynı ID birden fazla geldiyse teke düşür

            // Veritabanındaki kullanıcıları bul (Sadece bu ID'lere sahip olanları getirir)
            var existingUsers = await _userRepository.GetAsync(u => allUserIds.Contains(u.Id));

            // Eğer veritabanından gelen kullanıcı sayısı, bizim listemizdeki sayıyla eşleşmiyorsa, demek ki bazı ID'ler sahte!
            if (existingUsers.Count != allUserIds.Count)
            {
                // Global Exception Handler'ımızın yakalayabilmesi için FluentValidation hatası fırlatıyoruz
                throw new BusinessException("Gönderilen kullanıcı ID'lerinden bazıları sistemde bulunamadı.");
            }
            // --- KONTROL BİTİŞİ ---

            // 1. Grubu oluştur
            var group = new Group(request.Name, request.CreatedByUserId);
            await _groupRepository.AddAsync(group);

            // 2. Kullanıcıları ara tabloya ekle
            foreach (var userId in allUserIds)
            {
                var userGroup = new UserGroup(userId, group.Id);
                await _userGroupRepository.AddAsync(userGroup);
            }

            // 3. Tüm işlemleri tek bir Transaction ile veritabanına yansıt
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return group.Id;
        }
    }
}