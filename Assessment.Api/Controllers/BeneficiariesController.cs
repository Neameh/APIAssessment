using Assessment.Core.Logic.Beneficiaries.Command;
using Assessment.Core.Logic.Beneficiaries.Queries;
using Assessment.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Assessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BeneficiariesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(AddBeneficiaryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<BeneficiaryDto>>> Get(int userId)
        {
            var query = new GetBeneficiariesQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}


