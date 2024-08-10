using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	//The name dual arity comes from 2 arities, unary and binary
	public class DualArityOperationToken : IToken, IUnaryOperation, IBinaryOperation
    {
        public string DisplayName { get; init; } = "-";

		public Func<double, double, double> CalculateBinary { get; init; } = (x, y) => x - y;

		public Func<double, double> CalculateUnary { get; init; } = (x) => -x;

        public int Priority { get; init; } = int.MinValue;

		public Associativity Associativity { get; init; } = Associativity.Left;
	}
}
