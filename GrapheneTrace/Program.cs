using GrapheneTrace.Data;
using GrapheneTrace.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers + JSON support
builder.Services.AddControllers().AddNewtonsoftJson();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
                      ?? "Data Source=graphene.db"));

// Dependency Injection
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<FrameParserService>();
builder.Services.AddScoped<MetricsService>();
builder.Services.AddScoped<AlertService>();

var app = builder.Build();

// Dev Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
