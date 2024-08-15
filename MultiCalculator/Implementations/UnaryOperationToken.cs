using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	public class UnaryOperationToken : IToken, IUnaryOperation
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public Func<double, double> CalculateUnary { get; init; } = (x) => throw new MultiCalculatorException("Not implemented");

		public OperandPosition Position { get; init; } = OperandPosition.Prefix;
	}
}
