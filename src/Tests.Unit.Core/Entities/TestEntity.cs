namespace Tests.Unit.Core.Entities;

public class TestEntity
{
	public string StringProperty { get; set; }
	public int IntProperty { get; set; }
	public DateTime DateTimeProperty { get; set; }
	public TestEnum EnumProperty { get; set; }
}

public enum TestEnum
{
	Value1,
	Value2,
	Value3
}