global using NoobChessServer.Models;
global using NoobChessServer.Data;
using Microsoft.EntityFrameworkCore;
using NoobChessServer.Auth;
using NoobChessServer.Services.ArticlesService;

// Enable CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

// Add the DbContext
builder.Services.AddDbContext<DataContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreElephantSQL")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auto Mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Service DI
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IArticlesService, ArticlesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
