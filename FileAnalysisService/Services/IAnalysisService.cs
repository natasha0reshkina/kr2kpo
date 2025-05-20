using FileAnalysisService.Models;
using System.Threading.Tasks;

namespace FileAnalysisService.Services
{
    public interface IAnalysisService
    {
        Task<StatsDto> StatisticsAsync(FileContentDto input);
        Task<CompareResultDto> CompareAsync(FileContentDto input);
    }
}