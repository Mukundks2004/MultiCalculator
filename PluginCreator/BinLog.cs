using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace PluginCreator
{
	public class BinLog : IUnaryOperation
	{
		public Func<string, string> LatexString { get; init; } = (a) => "\\log_{2}{" + a + "}";

		public Func<double, double> CalculateUnary { get; init; } = Math.Log2;

		public Fixity Fixity { get; init; } = Fixity.Prefix;

		public int Priority { get; init; } = 2;

		public string TokenSymbol { get; init; } = "log2";
	}
}
