using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedPool.Application.Features.Groups.Commands.CreateGroup;

namespace SharedPool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupCommand command)
        {
            // İsteği MediatR'a gönderiyoruz. (Validator ve DB kontrolleri otomatik çalışacak)
            var groupId = await _mediator.Send(command);

            // İşlem başarılıysa 201 Created ve oluşan Grubun ID'sini dönüyoruz
            return Created(string.Empty, new { Id = groupId });
        }
    }
}
