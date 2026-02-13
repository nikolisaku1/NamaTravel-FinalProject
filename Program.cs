using Microsoft.EntityFrameworkCore;
using NamaTravelApi.Data;
using NamaTravelApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Skyscanner proxy service (HttpClient)
builder.Services.AddHttpClient<SkyscannerService>();

// CORS so your HTML pages (e.g., Live Server on 5500) can call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", p =>
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .SetIsOriginAllowed(_ => true) // simple for school project; lock down in real apps
         .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.MapControllers();

// Auto-create DB on first run (good for school demo)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
