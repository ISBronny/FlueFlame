using Microsoft.EntityFrameworkCore;
using Testing.TestData.AspNetCore.Models;

namespace Testing.TestData.AspNetCore.Database;

public sealed class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>()
            .HasData(new Employee(Guid.NewGuid())
                {
                    Age = 22,
                    Position = "Junior C# Developer",
                    FullName = "Joe Biden"
                },
                new Employee(Guid.NewGuid())
                {
                    Age = 55,
                    Position = "Senior Java Developer",
                    FullName = "Vladimir Parker"
                },
                new Employee(Guid.NewGuid())
                {
                    Age = 45,
                    Position = "DevOps",
                    FullName = "John Cena"
                });
    }

    public DbSet<Employee> Employees { get; set; }
}