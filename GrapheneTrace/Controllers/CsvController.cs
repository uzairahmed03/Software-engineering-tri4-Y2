using System;
using System.IO;
using System.Threading.Tasks;
using GrapheneTrace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneTrace.Controllers
{
    [ApiController]
    [Route("api/csv")]
    public class CsvController : ControllerBase
    {
        private readonly CsvImportService _import;
        private readonly CsvDashboardService _dash;

        public CsvController(CsvImportService import, CsvDashboardService dash)
        {
            _import = import;
            _dash = dash;
        }

        // POST /api/csv/import?overwrite=false
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromQuery] bool overwrite = false)
        {
            // CSVs placed at: GrapheneTrace/App_Data/CsvUploads/
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "CsvUploads");
            var result = await _import.ImportFolderAsync(folder, overwrite);
            return Ok(result);
        }

        // GET /api/csv/overview?date=2025-10-11
        [HttpGet("overview")]
        public async Task<IActionResult> Overview([FromQuery] DateTime? date = null)
        {
            return Ok(await _dash.GetOverviewAsync(date));
        }

        // GET /api/csv/colstats?sourceId=1c0fd777&date=2025-10-11&col=0
        [HttpGet("colstats")]
        public async Task<IActionResult> ColumnStats([FromQuery] string sourceId, [FromQuery] DateTime date, [FromQuery] int col)
        {
            var stats = await _dash.GetColumnStatsAsync(sourceId, date, col);
            if (stats is null) return NotFound(new { message = "No data found for given sourceId/date/col." });
            return Ok(stats);
        }

        // GET /api/csv/heatmap?sourceId=1c0fd777&date=2025-10-11&startRow=0&size=32
        [HttpGet("heatmap")]
        public async Task<IActionResult> Heatmap([FromQuery] string sourceId, [FromQuery] DateTime date,
            [FromQuery] int startRow = 0, [FromQuery] int size = 32)
        {
            var data = await _dash.GetHeatmapWindowAsync(sourceId, date, startRow, size);
            if (data is null) return NotFound(new { message = "No data found for given sourceId/date." });
            return Ok(data);
        }
    }
}
