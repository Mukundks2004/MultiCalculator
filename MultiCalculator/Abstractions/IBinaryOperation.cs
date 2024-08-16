using MultiCalculator.Enums;

namespace MultiCalculator.Abstractions
{
	public interface IBinaryOperation : IOperation
	{
		Func<double, double, double> CalculateBinary { get; init; }

		public Associativity Associativity { get; init; }
	}
}
