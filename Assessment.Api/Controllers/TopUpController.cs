using Assessment.Core.Dtos;
using Assessment.Core.Logic.Topups.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopUpController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TopUpController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<TopUpResult>> TopUp(TopUpCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}

