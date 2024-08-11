using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	public class BracketToken : IToken
    {
		static readonly BracketToken closedBracket = new() { DisplayName = ")", BracketType = BracketType.Closed };

		static readonly BracketToken openBracket = new() { DisplayName = "(", BracketType = BracketType.Open };

		public static BracketToken ClosedBracket { get => closedBracket; }

		public static BracketToken OpenBracket { get => openBracket; }

		public string DisplayName { get; init; } = "[";

		public BracketType BracketType { get; init; } = BracketType.Open;
    }
}
