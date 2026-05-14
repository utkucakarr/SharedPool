using FluentValidation;

namespace SharedPool.Application.Features.Groups.Commands.AddUserToGroup
{
    public class AddUserToGroupValidator : AbstractValidator<AddUserToGroupCommand>
    {
        public AddUserToGroupValidator()
        {
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("Grup ID boş olamaz.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı ID boş olamaz.");
        }
    }
}
