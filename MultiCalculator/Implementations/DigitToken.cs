using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class DigitToken : IToken
    {
        public string DisplayName { get; init; } = "1";

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
