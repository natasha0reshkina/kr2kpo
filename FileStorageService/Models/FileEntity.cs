using System;

namespace FileStorageService.Models
{

    public class FileEntity
    {
        public Guid Id          { get; set; }
        public string FileName  { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string Path      { get; set; } = default!;   
        public DateTime UploadedAt { get; set; }
    }
}