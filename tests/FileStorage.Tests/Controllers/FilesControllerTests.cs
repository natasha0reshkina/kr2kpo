using FileStorageService.Controllers;
using FileStorageService.Models;
using FileStorageService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

namespace FileStorage.Tests.Controllers
{
    public class FilesControllerTests
    {
        private readonly Mock<IFileRepository> _repoMock;
        private readonly FilesController _controller;

        public FilesControllerTests()
        {
            _repoMock = new Mock<IFileRepository>();
            _controller = new FilesController(_repoMock.Object);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsNull()
        {
            var result = await _controller.Upload(null);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Файл не выбран или пуст.", badRequest.Value);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsEmpty()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            var result = await _controller.Upload(mockFile.Object);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Файл не выбран или пуст.", badRequest.Value);
        }

        [Fact]
        public async Task Upload_ReturnsOk_WithId()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(10);
            var fileId = Guid.NewGuid();

            _repoMock.Setup(r => r.SaveAsync(mockFile.Object)).ReturnsAsync(fileId);

            var result = await _controller.Upload(mockFile.Object);
            var okResult = Assert.IsType<OkObjectResult>(result);

            var json = JsonSerializer.Serialize(okResult.Value);
            var parsed = JsonDocument.Parse(json);
            var actualId = parsed.RootElement.GetProperty("id").GetGuid();

            Assert.Equal(fileId, actualId);
        }

        [Fact]
        public async Task Download_ReturnsNotFound_WhenFileNotExists()
        {
            var fileId = Guid.NewGuid();
            _repoMock.Setup(r => r.GetAsync(fileId)).ReturnsAsync((StoredFile)null);

            var result = await _controller.Download(fileId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Download_ReturnsFile_WhenFileExists()
        {
            var fileId = Guid.NewGuid();
            var content = Encoding.UTF8.GetBytes("hello world");
            var stream = new MemoryStream(content);
            var storedFile = new StoredFile
            {
                Stream = stream,
                FileName = "hello.txt",
                ContentType = "text/plain"
            };

            _repoMock.Setup(r => r.GetAsync(fileId)).ReturnsAsync(storedFile);

            var result = await _controller.Download(fileId);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("text/plain", fileResult.ContentType);
            Assert.Equal("hello.txt", fileResult.FileDownloadName);
        }
    }
}
