using MediatR;
using SharedPool.Domain.Enums;
using SharedPool.Domain.Models;

namespace SharedPool.Application.Features.Expenses.Commands.CreateExpense
{
    public record CreateExpenseCommand(
        Guid GroupId,
        Guid PayerUserId, // Senin modelindeki isim
        string Description,
        decimal TotalAmount,
        DateTime ExpenseDate, // Senin modelindeki alan
        SplitType SplitType,
        List<SplitDetailModel> SplitDetails) : IRequest<Guid>;
}