namespace MultiCalculator.Abstractions
{
	//https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/interface?redirectedfrom=MSDN
	//TODO: replace with attribute- replace tostring with nameof
	public interface IToken
    {
		string TokenSymbol { get; init; }
    }
}
