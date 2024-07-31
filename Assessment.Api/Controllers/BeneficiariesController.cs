using Assessment.Core.Logic.Beneficiaries.Command;
using Assessment.Core.Logic.Beneficiaries.Queries;
using Assessment.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;


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
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<BeneficiaryDto>>> Get(int userId)
        {
            try
            {
                // Check if the user exists
                var userExists = await _mediator.Send(new CheckUserExistenceQuery { UserId = userId });
                if (!userExists)
                {
                    return NotFound($"User with ID {userId} not found.");
                }
                var query = new GetBeneficiariesQuery { UserId = userId };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


