using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	//The name dual arity comes from 2 arities, unary and binary
	public class DualArityOperationToken : IToken, IUnaryOperation, IBinaryOperation
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public Func<double, double, double> CalculateBinary { get; init; } = (x, y) => throw new MultiCalculatorException("Not implemented");

		public UnaryOperationToken UnaryOperation { get; init; } = new UnaryOperationToken();

		public Func<double, double> CalculateUnary { get => UnaryOperation.CalculateUnary; init => throw new MultiCalculatorException("Can't initialize CalculateUnary func, please set via UnaryOperation"); }

		public int Priority { get; init; } = int.MinValue;

		public Associativity Associativity { get; init; } = Associativity.Left;

		public OperandPosition Position { get; init; } = OperandPosition.Postfix;
	}
}
