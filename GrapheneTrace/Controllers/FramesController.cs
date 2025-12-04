using GrapheneTrace.DTOs;
using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/frames")]
    public class FramesController : ControllerBase
    {
        private readonly FrameParserService _service;

        public FramesController(FrameParserService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFrame(FrameUploadDto dto)
        {
            var metric = await _service.ProcessFrame(dto.UserId, dto.RawData);
            return Ok(metric);
        }
    }
}
