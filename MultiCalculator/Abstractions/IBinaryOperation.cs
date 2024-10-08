﻿using MultiCalculator.Enums;

namespace MultiCalculator.Abstractions
{
	public interface IBinaryOperation : IOperation
	{
		Func<string, string, string> LatexString { get; init; }

		Func<double, double, double> CalculateBinary { get; init; }

		public Associativity Associativity { get; init; }
	}
}
