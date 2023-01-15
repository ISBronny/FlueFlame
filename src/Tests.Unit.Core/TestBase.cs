using Tests.Unit.Core.Entities;

namespace Tests.Unit.Core;

public abstract class TestBase
{

	protected TestEntity TestEntity { get; } = TestEntityHelper.Random;


}