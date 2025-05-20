using FileAnalysisService.Data;
using FileAnalysisService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileAnalysisService.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly AnalysisDbContext _db;

        public AnalysisService(AnalysisDbContext db)
        {
            _db = db;
        }

        public Task<StatsDto> StatisticsAsync(FileContentDto input)
        {
            var text = input.Text ?? "";
            var paragraphs = text.Split('\n').Count(p => !string.IsNullOrWhiteSpace(p));
            var words = text.Split(' ', '\n', '\r').Count(w => !string.IsNullOrWhiteSpace(w));
            return Task.FromResult(new StatsDto(paragraphs, words, text.Length));
        }

        public async Task<CompareResultDto> CompareAsync(FileContentDto input)
        {
            var text = input.Text ?? "";

            // SHA256-хэш
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
            var hash = Convert.ToHexString(bytes);

        
            var exists = await _db.PlagiarismHashes
                .AsNoTracking()
                .AnyAsync(h => h.Hash == hash);
            if (exists)
                return new CompareResultDto(true);

   
            _db.PlagiarismHashes.Add(new PlagiarismHash { Hash = hash });
            await _db.SaveChangesAsync();
            return new CompareResultDto(false);
        }
    }
}