using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using FlightSimulatorAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>
    (options => options
    .UseMySql(builder.Configuration["MYSQL_CONNECTION_STRING"], new MySqlServerVersion(new Version(8, 0, 27)))
    .LogTo(Console.Write, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
    );
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.Run();
