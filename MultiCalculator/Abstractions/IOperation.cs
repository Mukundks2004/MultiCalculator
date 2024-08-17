namespace MultiCalculator.Abstractions
{
	public interface IOperation : IToken
	{
		int Priority { get; init; }
	}
}
