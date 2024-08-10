using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	public class BinaryOperationToken : IToken, IBinaryOperation
    {
        public string DisplayName { get; init; } = "&";

		public Func<double, double, double> CalculateBinary { get; init; } = (x, y) => x + y;

        public int Priority { get; init; } = int.MinValue;

		public Associativity Associativity { get; init; } = Associativity.Left;
	}
}
