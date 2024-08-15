﻿using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;

namespace MultiCalculator.Implementations
{
	public class BinaryOperationToken : IToken, IBinaryOperation
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public Func<double, double, double> CalculateBinary { get; init; } = (x, y) => throw new MultiCalculatorException("Not implemented");

        public int Priority { get; init; } = int.MinValue;

		public Associativity Associativity { get; init; } = Associativity.Left;
	}
}
