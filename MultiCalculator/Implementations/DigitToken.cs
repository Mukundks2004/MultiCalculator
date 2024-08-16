using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class DigitToken : IToken
    {
        public string TokenSymbol { get; init; } = string.Empty;

        public override string ToString()
        {
            return TokenSymbol;
        }
    }
}
