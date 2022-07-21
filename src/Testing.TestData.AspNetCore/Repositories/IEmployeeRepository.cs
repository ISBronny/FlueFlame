using Testing.TestData.AspNetCore.Models;

namespace Testing.TestData.AspNetCore.Repositories;

public interface IEmployeeRepository : IDisposable
{
    public Task<List<Employee>> GetAll();
    public Task<Employee> Add(Employee employee);

    public Task<Employee> GetById(Guid id);

    public Task<Employee> GetByFullName(string fullname);

    public Task Delete(Guid id);

    public int SaveChanges();
    public Task<int> SaveChangesAsync();
    Task<List<Employee>> OlderThan(int olderThan);
}