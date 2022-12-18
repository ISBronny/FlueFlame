using Examples.RestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Examples.RestApi.Database;

public sealed class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Employee> Employees { get; set; }
}