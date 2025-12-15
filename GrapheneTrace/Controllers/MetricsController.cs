using System;
using System.Threading.Tasks;
using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : ControllerBase
    {
        private readonly MetricsService _metrics;

        public MetricsController(MetricsService metrics)
        {
            _metrics = metrics;
        }

        // EXISTING ENDPOINT — DO NOT BREAK
        [HttpGet("latest")]
        public async Task<IActionResult> Latest()
        {
            var metric = await _metrics.GetLatestMetric();
            return Ok(metric);
        }

        // EXISTING ENDPOINT — DO NOT BREAK
        [HttpGet("range")]
        public async Task<IActionResult> Range(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var metrics = await _metrics.GetMetricsInRange(from, to);
            return Ok(metrics);
        }

        // NEW — ONE-TIME CSV → METRICS GENERATION
        [HttpPost("generate-from-csv")]
        public async Task<IActionResult> GenerateFromCsv(
            [FromQuery] string sourceId,
            [FromQuery] DateTime date)
        {
            await _metrics.GenerateMetricsFromCsvAsync(sourceId, date);
            return Ok(new { status = "metrics generated" });
        }
    }
}
