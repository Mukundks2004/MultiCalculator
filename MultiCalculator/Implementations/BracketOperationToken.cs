using MultiCalculator.Abstractions;
using MultiCalculator.Enums;

namespace MultiCalculator.Implementations
{
	public class BracketOperationToken : IToken
    {
        public string DisplayName { get; init; } = "[";

		public BracketType BracketType { get; init; } = BracketType.Open;
    }
}
