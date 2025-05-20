using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FileStorageService.Data;
using FileStorageService;
namespace FileStorage.Tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
           
                var desc = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<StorageDbContext>));
                if (desc != null) services.Remove(desc);


                services.AddDbContext<StorageDbContext>(opts =>
                    opts.UseInMemoryDatabase("TestDb"));
            });
        }
    }
}