using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrapheneTrace.Data;
using GrapheneTrace.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class CsvDashboardService
    {
        private readonly ApplicationDbContext _db;

        public CsvDashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns an overview of imported CSV datasets.
        /// SQLite cannot translate certain GroupBy + Distinct combinations,
        /// so we materialise minimal data first and complete grouping in memory.
        /// </summary>
        public async Task<List<CsvFileOverviewDto>> GetOverviewAsync(DateTime? date = null)
        {
            var q = _db.CsvSamples.AsNoTracking();

            if (date.HasValue)
            {
                var d = date.Value.Date;
                q = q.Where(x => x.MeasureDate == d);
            }

            // Materialise only what we need (SQLite-friendly)
            var rows = await q
                .Select(x => new
                {
                    x.SourceId,
                    x.MeasureDate,
                    x.RowIndex
                })
                .ToListAsync();

            // Complete grouping in memory
            return rows
                .GroupBy(x => new { x.SourceId, x.MeasureDate })
                .Select(g => new CsvFileOverviewDto(
                    g.Key.SourceId,
                    g.Key.MeasureDate,
                    g.Select(x => x.RowIndex).Distinct().Count(),
                    g.LongCount()
                ))
                .OrderBy(x => x.MeasureDate)
                .ThenBy(x => x.SourceId)
                .ToList();
        }

        /// <summary>
        /// Returns min/max/avg statistics for a specific column.
        /// This query is SQLite-safe and fully server-side.
        /// </summary>
        public async Task<CsvColumnStatsDto?> GetColumnStatsAsync(
            string sourceId,
            DateTime date,
            int colIndex)
        {
            var d = date.Date;

            var q = _db.CsvSamples.AsNoTracking()
                .Where(x =>
                    x.SourceId == sourceId &&
                    x.MeasureDate == d &&
                    x.ColIndex == colIndex);

            if (!await q.AnyAsync())
                return null;

            return await q
                .GroupBy(_ => 1)
                .Select(g => new CsvColumnStatsDto(
                    sourceId,
                    d,
                    colIndex,
                    g.LongCount(),
                    g.Min(x => x.Value),
                    g.Max(x => x.Value),
                    g.Average(x => x.Value)
                ))
                .FirstAsync();
        }

        /// <summary>
        /// Returns a 32x32 (or configurable size) heatmap window.
        /// This endpoint is used directly by the dashboard UI.
        /// </summary>
        public async Task<CsvHeatmapWindowDto?> GetHeatmapWindowAsync(
            string sourceId,
            DateTime date,
            int startRow,
            int size)
        {
            if (size <= 0 || size > 256)
                size = 32;

            if (startRow < 0)
                startRow = 0;

            var d = date.Date;
            int endRow = startRow + size - 1;

            var rows = await _db.CsvSamples.AsNoTracking()
                .Where(x =>
                    x.SourceId == sourceId &&
                    x.MeasureDate == d &&
                    x.RowIndex >= startRow &&
                    x.RowIndex <= endRow)
                .Select(x => new
                {
                    x.RowIndex,
                    x.ColIndex,
                    x.Value
                })
                .ToListAsync();

            if (rows.Count == 0)
                return null;

            var matrix = new double[size][];

            for (int r = 0; r < size; r++)
            {
                matrix[r] = new double[32];
                for (int c = 0; c < 32; c++)
                    matrix[r][c] = double.NaN;
            }

            foreach (var item in rows)
            {
                int rr = item.RowIndex - startRow;
                if (rr >= 0 && rr < size &&
                    item.ColIndex >= 0 && item.ColIndex < 32)
                {
                    matrix[rr][item.ColIndex] = item.Value;
                }
            }

            return new CsvHeatmapWindowDto(
                sourceId,
                d,
                startRow,
                size,
                matrix);
        }
    }
}
