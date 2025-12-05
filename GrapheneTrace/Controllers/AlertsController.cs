using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly AlertService _service;

        public AlertsController(AlertService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlerts()
        {
            return Ok(await _service.GetAlerts());
        }

        [HttpPost("{id}/acknowledge")]
        public async Task<IActionResult> Acknowledge(int id)
        {
            var ok = await _service.AcknowledgeAlert(id);
            if (!ok) return NotFound("Alert not found.");

            return Ok("Alert acknowledged.");
        }
    }
}
