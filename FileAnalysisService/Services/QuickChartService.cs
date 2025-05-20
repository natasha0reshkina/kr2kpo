using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileAnalysisService.Services;

public class QuickChartService
{
    private readonly HttpClient _http;
    public QuickChartService(HttpClient http) => _http = http;

    public async Task<byte[]> GenerateWordCloudAsync(string text)
    {
        var uri = $"https://quickchart.io/wordcloud?text={Uri.EscapeDataString(text)}";
        return await _http.GetByteArrayAsync(uri);
    }
}