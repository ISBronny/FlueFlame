using Examples.Domain.Models;
using Examples.Infrastructure.Database;

namespace Examples.Tests.Api.TestDataBuilders;

public class EmployeeTestDataBuilder
{
	private Guid? _guid;
	private int? _age;
	private string _position;
	private string _fullName;
	
	protected EmployeeContext Context { get; }

	public EmployeeTestDataBuilder(EmployeeContext context = null)
	{
		Context = context;
	}
	
	public EmployeeTestDataBuilder WithGuid(Guid guid)
	{
		_guid = guid;
		return this;
	}
	
	public EmployeeTestDataBuilder WithAge(int age)
	{
		_age = age;
		return this;
	}

	public EmployeeTestDataBuilder WithFullName(string fullName)
	{
		_fullName = fullName;
		return this;
	}
	
	public EmployeeTestDataBuilder WithPosition(string position)
	{
		_position = position;
		return this;
	}

	public Employee Build(bool saveInDb = true)
	{
		var employee = new Employee(_guid ?? Guid.NewGuid())
		{
			Age = _age ?? Faker.RandomNumber.Next(18, 100),
			Position = _position ?? "Php Senior Developer",
			FullName = _fullName ?? Faker.Name.FullName(),
		};

		if (!saveInDb) 
			return employee;
		
		Context.Employees.Add(employee);
		Context.SaveChanges();

		return employee;
	}
}