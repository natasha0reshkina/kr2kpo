namespace FileAnalysisService.Models
{
    public class PlagiarismHash
    {
        public int Id    { get; set; }
        public string Hash { get; set; } = default!;
    }
}