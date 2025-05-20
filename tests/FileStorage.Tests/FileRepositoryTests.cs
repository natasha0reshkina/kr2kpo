using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileStorageService.Data;
using FileStorageService.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore; 
namespace FileStorage.Tests;

public class FileRepositoryTests
{
    private static IFileRepository CreateRepo(string root)
    {

        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(root);

        var opts = new DbContextOptionsBuilder<StorageDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new StorageDbContext(opts);

        return new FileRepository(env.Object, db);
    }

    [Fact]
    public async Task Save_and_Get_roundtrip()
    {
        var temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(temp);

        var repo = CreateRepo(temp);

   
        var content = "hello";
        var data = Encoding.UTF8.GetBytes(content);
        await using var ms = new MemoryStream(data);
        var formFile = new FormFile(ms, 0, data.Length, "unused", "greet.txt")
        {
            Headers     = new HeaderDictionary(),
            ContentType = "text/plain"
        };

       
        var id     = await repo.SaveAsync(formFile);
        var result = await repo.GetAsync(id);


        result.Should().NotBeNull();
        result!.FileName.Should().Be("greet.txt");
        using var sr = new StreamReader(result.Stream);
        (await sr.ReadToEndAsync()).Should().Be(content);
        
        Directory.Delete(temp, true);
    }
}