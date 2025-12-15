using System;
using System.Collections.Generic;

namespace GrapheneTrace.DTOs
{
    public record CsvImportResultDto(
        int FilesFound,
        int FilesImported,
        int FilesSkipped,
        long SamplesInserted,
        List<string> Messages
    );

    public record CsvFileOverviewDto(
        string SourceId,
        DateTime MeasureDate,
        int DistinctRows,
        long SampleCount
    );

    public record CsvColumnStatsDto(
        string SourceId,
        DateTime MeasureDate,
        int ColIndex,
        long Count,
        double Min,
        double Max,
        double Avg
    );

    public record CsvHeatmapWindowDto(
        string SourceId,
        DateTime MeasureDate,
        int StartRow,
        int Size,
        double[][] Values // size x 32
    );
}
