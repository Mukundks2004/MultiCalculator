using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class UnaryOperationToken : IToken, IUnaryOperation
    {
        public string DisplayName { get; init; } = "f(x)";

		public Func<double, double> CalculateUnary { get; init; } = (x) => x;
    }
}
