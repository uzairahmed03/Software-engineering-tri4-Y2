using GrapheneTrace.DTOs;
using GrapheneTrace.Services;
using GrapheneTrace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/frames")]
    public class FramesController : ControllerBase
    {
        private readonly FrameParserService _service;
        private readonly ApplicationDbContext _db;

        public FramesController(FrameParserService service, ApplicationDbContext db)
        {
            _service = service;
            _db = db;
        }

        // ─────────────────────────────────────────────
        // EXISTING ENDPOINT (DO NOT TOUCH)
        // POST /api/frames/upload
        // ─────────────────────────────────────────────
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFrame(FrameUploadDto dto)
        {
            var metric = await _service.ProcessFrame(dto.UserId, dto.RawData);
            return Ok(metric);
        }

        // ─────────────────────────────────────────────
        // NEW ENDPOINT (FOR HEATMAP VIEWER)
        // GET /api/frames?sourceId=XXX&date=YYYY-MM-DD&frame=0
        // ─────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetFrame(
            [FromQuery] string sourceId,
            [FromQuery] DateTime date,
            [FromQuery] int frame)
        {
            var values = await _db.CsvSamples
                .Where(s =>
                    s.SourceId == sourceId &&
                    s.MeasureDate == date &&
                    s.RowIndex == frame)
                .OrderBy(s => s.ColIndex)
                .Select(s => s.Value)
                .ToListAsync();

            if (!values.Any())
                return NotFound();

            return Ok(values);
        }
    }
}
