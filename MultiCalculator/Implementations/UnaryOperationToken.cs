using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	public class UnaryOperationToken : IUnaryOperation
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public Func<string, string> LatexString { get; init; } = (x) => throw new MultiCalculatorException("Not implemented");

		public Func<double, double> CalculateUnary { get; init; } = (x) => throw new MultiCalculatorException("Not implemented");

		public int Priority { get; init; } = int.MinValue;

		public Fixity Fixity { get; init; } = Fixity.Prefix;
	}
}
