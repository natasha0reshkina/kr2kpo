using FileStorageService.Models;
using FileStorageService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FileStorageService.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly IFileRepository _repo;

        public FilesController(IFileRepository repo)
        {
            _repo = repo;
        }

      
        [HttpPost]
        [Consumes("multipart/form-data")]            // ← добавьте это
        public async Task<IActionResult> Upload(
            [FromForm(Name = "file")] IFormFile file) // ← и убедитесь, что это IFormFile
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран или пуст.");

            var id = await _repo.SaveAsync(file);
            return Ok(new { id });
        }

        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var stored = await _repo.GetAsync(id);
            if (stored == null)
                return NotFound();

            return File(stored.Stream, stored.ContentType, stored.FileName);
        }
    }
}