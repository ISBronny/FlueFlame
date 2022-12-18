using Examples.RestApi.Database;
using Examples.RestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Examples.RestApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeContext _context;

    public EmployeeRepository(EmployeeContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAll()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> Add(Employee employee)
    {
        return _context.Employees.Add(employee).Entity;
    }

    public async Task<Employee> GetById(Guid id)
    {
        return await _context.Employees.SingleOrDefaultAsync(x => x.Guid == id);
    }
    
    public async Task<Employee> GetByFullName(string fullname)
    {
        return await _context.Employees.SingleOrDefaultAsync(x => x.FullName == fullname);
    }

    public async Task Delete(Guid id)
    {
        _context.Employees.Remove(await _context.Employees.FindAsync(id) ?? throw new InvalidOperationException());
    }
    
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<List<Employee>> OlderThan(int olderThan)
    {
        return await _context.Employees.Where(x => x.Age > olderThan).ToListAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}