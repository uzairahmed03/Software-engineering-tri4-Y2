using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : ControllerBase
    {
        private readonly MetricsService _service;

        public MetricsController(MetricsService service)
        {
            _service = service;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> Latest()
        {
            return Ok(await _service.GetLatestMetric());
        }

        [HttpGet("range")]
        public async Task<IActionResult> Range(DateTime start, DateTime end)
        {
            return Ok(await _service.GetMetricsInRange(start, end));
        }
    }
}
