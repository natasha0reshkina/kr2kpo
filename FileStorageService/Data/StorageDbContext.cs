using FileStorageService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Data
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> opts)
            : base(opts)
        {
        }

        public DbSet<FileEntity> FileEntities { get; set; } = default!;
    }
}