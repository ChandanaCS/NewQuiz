using BLL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();

//Dependency Injection for DB context class
builder.Services.AddDbContext<QuizDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure data protection
builder.Services.AddDataProtection();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Update with your Redis server configuration
});

// Configure session services
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".My.Session.Cookie";
    options.IdleTimeout = TimeSpan.FromSeconds(1000); // Adjust the timeout as needed
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
