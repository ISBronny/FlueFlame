using MathGrpcService;

namespace Testing.TestData.AspNetCore.Helpers;

public static class NumberHelper
{
	public static Number FromInteger(int value)
	{
		return new Number {Value = value};
	}
}