using FlightSimulatorAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var toto = builder.Configuration["MYSQL_CONNECTION_STRING"];

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>
    (options => options
    .UseMySql(builder.Configuration["MYSQL_CONNECTION_STRING"], new MySqlServerVersion(new Version(8, 0, 27)))
    .LogTo(Console.Write, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
    );

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.Run();
