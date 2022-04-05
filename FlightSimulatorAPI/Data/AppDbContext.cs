using FlightSimulatorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSimulatorAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Aircraft> Aircrafts { get; set; }
    public DbSet<Result> Results { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
