namespace Testing.Tests.UnitTests.Entities;

public static class TestEntityHelper
{
	public static TestEntity Random =>
		new()
		{
			IntProperty = Faker.RandomNumber.Next(),
			EnumProperty = Faker.Enum.Random<TestEnum>(),
			StringProperty = Faker.Name.FullName(),
			DateTimeProperty = Faker.Identification.DateOfBirth(),
		};

}