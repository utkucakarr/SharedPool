using MediatR;
using SharedPool.Domain.Entities;
using SharedPool.Domain.Interfaces;

namespace SharedPool.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // NOT: Gerçek projede şifre ASLA düz metin (plain text) kaydedilmez.
            // İleride BCrypt veya ASP.NET Core Identity PasswordHasher ile burayı revize etmeliyiz.
            // Şimdilik mimariyi ayağa kaldırmak için doğrudan alıyoruz.

            var user = new User(request.FirstName, request.LastName, request.Email, request.Password);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}