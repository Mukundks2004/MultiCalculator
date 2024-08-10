using MultiCalculator.Enums;

namespace MultiCalculator.Abstractions
{
	public interface IBinaryOperation
	{
		Func<double, double, double> CalculateBinary { get; init; }

		int Priority { get; init; }

		public Associativity Associativity { get; init; }
	}
}
