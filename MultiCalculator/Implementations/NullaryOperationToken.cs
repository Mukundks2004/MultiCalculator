using MultiCalculator.Abstractions;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	public class NullaryOperationToken : IToken
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public string LatexString { get; init; } = string.Empty;

		public Func<double> Calculate { get; init; } = () => throw new MultiCalculatorException("Not implemented");

		public static NullaryOperationToken GetConstFromDouble(double value = 0)
		{
			return new NullaryOperationToken() { Calculate = () => value, TokenSymbol = value.ToString(), LatexString = value.ToString() };
		}
	}
}
