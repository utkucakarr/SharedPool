using FluentValidation;

namespace SharedPool.Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseValidator()
        {
            RuleFor(x => x.GroupId).NotEmpty().WithMessage("Grup ID boş olamaz.");
            RuleFor(x => x.PayerUserId).NotEmpty().WithMessage("Ödeyen kişi ID boş olamaz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz.").
                MaximumLength(200).WithMessage("\"Açıklama 200 karakteri geçemez.");
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Fatura tutarı 0'dan büyük olmalıdır.");
            RuleFor(x => x.SplitDetails).NotEmpty().WithMessage("Bölüşüm detayları boş olamaz.");
        }
    }
}
