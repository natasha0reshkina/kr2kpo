using System;
using System.Net.Http;
using System.Threading.Tasks;
using FileAnalysisService.Models;

namespace FileAnalysisService.Services
{
    public class QuickChartService
    {
        private readonly HttpClient _http;

        public QuickChartService(HttpClient http)
            => _http = http;

        public async Task<byte[]> GetWordCloudAsync(FileContentDto dto)
        {
          
            var text  = Uri.EscapeDataString(dto.Text);
            var url   = $"https://quickchart.io/wordcloud?text={text}";
            
            var res   = await _http.GetAsync(url);
            res.EnsureSuccessStatusCode();

            return await res.Content.ReadAsByteArrayAsync();
        }
    }
}