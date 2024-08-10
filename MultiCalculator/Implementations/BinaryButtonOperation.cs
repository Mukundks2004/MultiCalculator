using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class BinaryButtonOperation : IButtonOperation
    {
        public string DisplayName { get; init; } = "&";

		public Func<double, double, double> Calculate { get; init; } = (x, y) => x + y;

        public int Priority { get; init; } = int.MinValue;

		public bool IsUnary { get; init; } = false;
	}
}
