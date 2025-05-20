using System;
using System.Threading.Tasks;
using FileAnalysisService.Data;
using FileAnalysisService.Models;
using FileAnalysisService.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FileAnalysis.Tests;

public class CompareTests
{
    private static AnalysisService CreateService(string dbName)
    {
        var opts = new DbContextOptionsBuilder<AnalysisDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new AnalysisService(new AnalysisDbContext(opts));
    }

    [Fact]
    public async Task First_upload_is_not_duplicate()
    {
        var svc = CreateService(Guid.NewGuid().ToString());
        var result = await svc.CompareAsync(new FileContentDto("alpha"));

        result.IsDuplicate.Should().BeFalse();
    }

    [Fact]
    public async Task Same_content_twice_is_marked_duplicate()
    {
        var dbName = Guid.NewGuid().ToString();

        var svc1 = CreateService(dbName);
        await svc1.CompareAsync(new FileContentDto("alpha"));

        var svc2 = CreateService(dbName);
        var result = await svc2.CompareAsync(new FileContentDto("alpha"));

        result.IsDuplicate.Should().BeTrue();
    }
}