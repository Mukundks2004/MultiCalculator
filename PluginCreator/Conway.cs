using MultiCalculator.Abstractions;

namespace PluginCreator
{
	public class Conway : INullaryOperation
	{
		public string LatexString { get; init; } = "\\lambda";

		public Func<double> Calculate { get; init; } = () => 1.303577269034;

		public string TokenSymbol { get; init; } = "λ";
	}
}
