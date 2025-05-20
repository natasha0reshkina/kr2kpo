using FileStorageService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FileStorageService.Repositories
{
    public interface IFileRepository
    {
        /// <summary>
        /// Сохраняет IFormFile на диск и в БД, возвращает его Guid.
        /// </summary>
        Task<Guid> SaveAsync(IFormFile file);

        /// <summary>
        /// Возвращает StoredFile по Guid, или null если не найден.
        /// </summary>
        Task<StoredFile?> GetAsync(Guid id);
    }
}