using GrapheneTrace.Data;
using GrapheneTrace.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // we can add NewtonsoftJson later if needed

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=graphene.db"));

// DI for services
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<FrameParserService>();
builder.Services.AddScoped<MetricsService>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<CsvImportService>();
builder.Services.AddScoped<CsvDashboardService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve index.html + static assets from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
