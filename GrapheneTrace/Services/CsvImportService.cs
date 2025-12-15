using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrapheneTrace.Data;
using GrapheneTrace.Models;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTrace.Services
{
    public class CsvImportService
    {
        private readonly ApplicationDbContext _db;
        private readonly MetricsService _metrics;

        private const int BATCH_SIZE = 5000;

        public CsvImportService(
            ApplicationDbContext db,
            MetricsService metrics)
        {
            _db = db;
            _metrics = metrics;
        }

        // =====================================================
        // EXISTING API — DO NOT BREAK CONTROLLERS
        // =====================================================

        // folder only
        public async Task ImportFolderAsync(string folderPath)
        {
            await ImportDirectoryAsync(folderPath, null, false);
        }

        // folder + sourceId
        public async Task ImportFolderAsync(string folderPath, string sourceId)
        {
            await ImportDirectoryAsync(folderPath, sourceId, false);
        }

        // folder + overwrite — CONTROLLER EXPECTS A RETURN VALUE
        public async Task<bool> ImportFolderAsync(string folderPath, bool overwrite)
        {
            await ImportDirectoryAsync(folderPath, null, overwrite);
            return true;
        }

        // =====================================================
        // INTERNAL IMPLEMENTATION
        // =====================================================
        private async Task ImportDirectoryAsync(
            string directoryPath,
            string? forcedSourceId,
            bool overwrite)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(directoryPath);

            var files = Directory.GetFiles(directoryPath, "*.csv");

            foreach (var file in files)
            {
                await ImportSingleFileAsync(file, forcedSourceId, overwrite);
            }
        }

        private async Task ImportSingleFileAsync(
            string filePath,
            string? forcedSourceId,
            bool overwrite)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // Expected format: sourceId_yyyy-MM-dd.csv
            var parts = fileName.Split('_');
            if (parts.Length < 2)
                return;

            string sourceId = forcedSourceId ?? parts[0];

            if (!DateTime.TryParseExact(
                parts[1],
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime measureDate))
                return;

            bool exists = await _db.CsvSamples.AnyAsync(x =>
                x.SourceId == sourceId &&
                x.MeasureDate == measureDate);

            if (exists && !overwrite)
                return;

            if (exists && overwrite)
            {
                _db.CsvSamples.RemoveRange(
                    _db.CsvSamples.Where(x =>
                        x.SourceId == sourceId &&
                        x.MeasureDate == measureDate));

                _db.Metrics.RemoveRange(
                    _db.Metrics.Where(m =>
                        m.Timestamp.Date == measureDate.Date));

                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }

            using var reader = new StreamReader(filePath);

            int rowIndex = 0;
            var batch = new System.Collections.Generic.List<CsvSample>(BATCH_SIZE);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',');

                for (int col = 0; col < values.Length; col++)
                {
                    if (!double.TryParse(
                        values[col],
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out var v))
                        continue;

                    batch.Add(new CsvSample
                    {
                        SourceId = sourceId,
                        MeasureDate = measureDate,
                        RowIndex = rowIndex,
                        ColIndex = col,
                        Value = v
                    });
                }

                rowIndex++;

                if (batch.Count >= BATCH_SIZE)
                {
                    _db.CsvSamples.AddRange(batch);
                    await _db.SaveChangesAsync();
                    _db.ChangeTracker.Clear();
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                _db.CsvSamples.AddRange(batch);
                await _db.SaveChangesAsync();
                _db.ChangeTracker.Clear();
            }

            // 🔥 AUTOMATIC METRICS GENERATION
            await _metrics.GenerateMetricsFromCsvAsync(sourceId, measureDate);
        }
    }
}
