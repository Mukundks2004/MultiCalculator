using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	//The name dual arity comes from 2 arities, unary and binary
	//Dual arity operators in practice can be prefix or postfix... we need to account for this.
	//We can account for this by setting a reference to a unary and a binary operator to make a dual arity
	public class DualArityOperationToken : IToken, IUnaryOperation, IBinaryOperation
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public Func<double, double, double> CalculateBinary { get; init; } = (x, y) => throw new MultiCalculatorException("Not implemented");

		public UnaryOperationToken UnaryOperation { get; init; } = new UnaryOperationToken();

		public Func<double, double> CalculateUnary { get => UnaryOperation.CalculateUnary; init { } }

		public int Priority { get; init; } = int.MinValue;

		public Associativity Associativity { get; init; } = Associativity.Left;

		public Fixity Fixity { get; init; } = Fixity.Prefix;
	}
}
