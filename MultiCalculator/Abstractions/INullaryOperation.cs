namespace MultiCalculator.Abstractions
{
	public interface INullaryOperation : IToken
	{
		string LatexString { get; init; }

		Func<double> Calculate { get; init; }
	}
}
