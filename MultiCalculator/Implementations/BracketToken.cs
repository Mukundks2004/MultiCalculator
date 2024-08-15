using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	//TODO: after IToken has been replaced with attribute, make this an enum
	public class BracketToken : IToken
    {
		public string TokenSymbol { get; init; } = string.Empty;

		public BracketType BracketType { get; init; } = BracketType.Open;
    }
}
