using Studex.Models;
using Microsoft.EntityFrameworkCore;
using Studex.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudexContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudexContext")));

builder.Services.AddControllers();

builder.Services.AddScoped<CourseService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();