using Examples.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Examples.Infrastructure.Database;

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