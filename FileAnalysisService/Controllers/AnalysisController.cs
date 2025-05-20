using FileAnalysisService.Models;
using FileAnalysisService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileAnalysisService.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysis;

        public AnalysisController(IAnalysisService analysis)
        {
            _analysis = analysis;
        }

        [HttpPost("stats")]
        public async Task<IActionResult> Statistics([FromBody] FileContentDto dto)
            => Ok(await _analysis.StatisticsAsync(dto));

        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] FileContentDto dto)
            => Ok(await _analysis.CompareAsync(dto));
    }
}