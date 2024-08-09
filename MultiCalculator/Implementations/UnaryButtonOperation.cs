using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class UnaryButtonOperation : IButtonOperation
    {
        public string DisplayName { get; init; } = "f(x)";

		public Func<double, double> Calculate { get; init; } = (x) => x;
    }
}
