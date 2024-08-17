using MultiCalculator.Enums;

namespace MultiCalculator.Abstractions
{
	public interface IUnaryOperation : IOperation
	{
		Func<string, string> LatexString { get; init; }

		Func<double, double> CalculateUnary { get; init; }

		Fixity Fixity { get; init; }
	}
}
