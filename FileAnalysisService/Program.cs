using FileAnalysisService.Data;
using FileAnalysisService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Подхватываем строку подключения из ENV (.env через docker-compose)
builder.Services.AddDbContext<AnalysisDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Наш сервис анализа
builder.Services.AddScoped<IAnalysisService, AnalysisService>();

// 3) HttpClient для QuickChart (если используете облако слов)
builder.Services.AddHttpClient<QuickChartService>();

// 4) MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AnalysisDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
namespace FileAnalysisService
{
    public partial class Program { }
}
