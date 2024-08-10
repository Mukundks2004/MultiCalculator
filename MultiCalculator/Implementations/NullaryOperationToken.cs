using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class NullaryOperationToken : IToken
    {
        public string DisplayName { get; init; } = "T";

		public Func<double> Calculate { get; init; } = () => 0;
    }
}
