using FluentValidation;

namespace SharedPool.Application.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Grup adı boş olamaz.")
                .MaximumLength(100).WithMessage("Grup adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.CreatedByUserId)
                .NotEmpty().WithMessage("Grubu oluşturan kullanıcı ID'si boş olamaz.");

            RuleFor(x => x.MemberIds)
                .NotNull().WithMessage("Üye listesi null olamaz.");
        }
    }
}