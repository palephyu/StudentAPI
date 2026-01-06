using Microsoft.EntityFrameworkCore;
using StudentApi.Data;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register HttpClient factory
builder.Services.AddHttpClient();

// Register CORS so MVC (different origin/port) can call API during dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", p =>
        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext
builder.Services.AddDbContext<StudentDbContext>(o => o.UseSqlServer(config.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocal");

app.UseAuthorization();

app.MapControllers();

app.Run();
