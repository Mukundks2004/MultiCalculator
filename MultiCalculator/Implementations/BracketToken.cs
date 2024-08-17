using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	//TODO: after IToken has been replaced with attribute, make this an enum
	//For the meantime, singleton pattern
	public class BracketToken : IToken
    {
		static readonly BracketToken closedBracket = new() { BracketType = BracketType.Closed, TokenSymbol = "]", LatexString = "\\right)" };
		static readonly BracketToken openBracket = new() { BracketType = BracketType.Open, TokenSymbol = "[", LatexString = "\\left(" };

		BracketToken()
		{
		}

		public string TokenSymbol { get; init; } = string.Empty;

		public BracketType BracketType { get; init; } = BracketType.Open;

		public static BracketToken ClosedBracket { get => closedBracket; }

		public static BracketToken OpenBracket { get => openBracket; }

		public string LatexString { get; init; } = string.Empty;
	}
}
