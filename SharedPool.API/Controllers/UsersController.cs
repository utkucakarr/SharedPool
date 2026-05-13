using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedPool.Application.Features.Users.Commands.CreateUser;

namespace SharedPool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            // İsteği MediatR'a gönderiyoruz. 
            // Arka planda önce yazdığımız ValidationBehavior çalışacak. 
            // Hata yoksa CreateUserCommandHandler çalışıp veritabanına kayıt atacak.
            var userId = await _mediator.Send(command);

            // Başarılı olursa HTTP 201 Created ve oluşturulan kaynağın Id'sini dönüyoruz.
            return Created(string.Empty, new { Id = userId });
        }
    }
}
