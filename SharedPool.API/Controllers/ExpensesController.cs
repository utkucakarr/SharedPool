using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedPool.Application.Features.Expenses.Commands.CreateExpense;

namespace SharedPool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpensesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand command)
        {
            // İsteği MediatR aracılığıyla ilgili Handler'a gönderiyoruz
            var expenseId = await _mediator.Send(command);

            // Başarılı olursa 201 Created ve oluşturulan Harcamanın Id'sini dönüyoruz
            return Created(string.Empty, new { Id = expenseId });
        }
    }
}
