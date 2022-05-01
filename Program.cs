using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Back.Entities;
using Back.Models;
using Back.Services;
using Back;

using Back.Models.Validators;
using Back.Middleware;

var builder = WebApplication.CreateBuilder(args);

//NLog w/Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// Services

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApiSeeder>();
builder.Services.AddScoped<IValidator<CreateCharacterDto>, CreateCharacterDtoValidator>();
builder.Services.AddScoped<IValidator<CharacterQuery>, CharacterQueryValidator>();
builder.Services.AddScoped<ErrorHandling>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApiDbContext>();
builder.Services.AddScoped<ICharacterService, CharacterService>();


//open cors for testing purposes
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>
    builder.AllowAnyMethod()
           .AllowAnyHeader()
           .AllowAnyOrigin()
    );
});

var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<ApiSeeder>();
seeder.seed();

app.UseCors("FrontEndClient");


// Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandling>();

app.UseHttpsRedirection();

app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
