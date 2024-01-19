using ArrayScorer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ArrayScorer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTotalScoresController : ControllerBase
    {
        private readonly IGetTotalScoreService _getTotalScoreService;

        public GetTotalScoresController(IGetTotalScoreService getTotalScoreService)
        {
            _getTotalScoreService = getTotalScoreService ?? throw new ArgumentNullException(nameof(getTotalScoreService));
        }

        [HttpPost("GetTotalScore")]
        public IActionResult GetTotalScore([FromBody] List<int> request)
        {
            try
            {
                if (request == null || !request.Any())
                {
                    return BadRequest("Invalid input. The request must be a non-empty list of integers.");
                }

                var totalResult = _getTotalScoreService.GetTotalScore(request);

                return totalResult.IsSuccessful
                    ? Ok(totalResult.Result)
                    : BadRequest(totalResult.Result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
