using Microsoft.AspNetCore.Mvc;

namespace Assessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopUpOptionsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<decimal>> Get()
        {
            var topUpOptions = new List<decimal> { 5, 10, 20, 30, 50, 75, 100 };
            return Ok(topUpOptions);
        }
    }
}


