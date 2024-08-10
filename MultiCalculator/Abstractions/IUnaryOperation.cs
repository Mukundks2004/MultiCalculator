namespace MultiCalculator.Abstractions
{
	public interface IUnaryOperation
	{
		Func<double, double> CalculateUnary { get; init; }
	}
}
