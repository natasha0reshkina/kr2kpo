using FileStorageService.Data;
using FileStorageService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileStorageService.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _storageFolder;
        private readonly StorageDbContext _db;

        public FileRepository(IWebHostEnvironment env, StorageDbContext db)
        {
            _storageFolder = Path.Combine(env.ContentRootPath, "StoredFiles");
            Directory.CreateDirectory(_storageFolder);
            _db = db;
        }

        public async Task<Guid> SaveAsync(IFormFile file)
        {
            var id = Guid.NewGuid();
            // — файл без расширения, путь = GUID
            var relPath = id.ToString();
            var fullPath = Path.Combine(_storageFolder, relPath);

            // 1) Сохраняем на диск
            await using (var fs = new FileStream(fullPath, FileMode.CreateNew))
                await file.CopyToAsync(fs);

            // 2) Пишем в базу
            var entity = new FileEntity
            {
                Id          = id,
                FileName    = file.FileName,
                ContentType = file.ContentType,
                Path        = relPath,
                UploadedAt  = DateTime.UtcNow
            };
            _db.FileEntities.Add(entity);
            await _db.SaveChangesAsync();

            return id;
        }

        public async Task<StoredFile?> GetAsync(Guid id)
        {
            var entity = await _db.FileEntities.FindAsync(id);
            if (entity == null) return null;

            var fullPath = Path.Combine(_storageFolder, entity.Path);
            if (!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new StoredFile
            {
                Stream      = stream,
                ContentType = entity.ContentType,
                FileName    = entity.FileName
            };
        }
    }
}
