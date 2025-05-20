using System.IO;

namespace FileStorageService.Models
{

    public class StoredFile
    {
        public Stream Stream        { get; init; } = default!;
        public string ContentType   { get; init; } = default!;
        public string FileName      { get; init; } = default!;
    }
}