using System.Threading.Tasks;
using FileAnalysisService.Data;
using FileAnalysisService.Models;
using FileAnalysisService.Services;
using FluentAssertions;
using FileAnalysisService.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FileAnalysis.Tests;

public class StatisticsTests
{
    private static AnalysisService CreateService()
    {
        var opts = new DbContextOptionsBuilder<AnalysisDbContext>()
            .UseInMemoryDatabase("StatsDb")
            .Options;
        var db = new AnalysisDbContext(opts);
        return new AnalysisService(db);
    }

    [Theory]
    [InlineData("Hello world",      1, 2, 11)]
    [InlineData("One\n\nTwo three", 2, 3, 14)]
    public async Task Counts_are_correct(string raw, int paras, int words, int chars)
    {
        var svc = CreateService();
        var dto = new FileContentDto(raw);

        var stats = await svc.StatisticsAsync(dto);

        stats.Paragraphs.Should().Be(paras);
        stats.Words.Should().Be(words);
        stats.Characters.Should().Be(chars);
    }
}