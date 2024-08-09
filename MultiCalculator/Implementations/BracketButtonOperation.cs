using MultiCalculator.Abstractions;

namespace MultiCalculator.Implementations
{
	public class BracketButtonOperation : IButtonOperation
    {
        public string DisplayName { get; init; } = "[";

		public BracketType BracketType { get; init; } = BracketType.Open;
    }

    public enum BracketType
    {
        Open,
        Closed
    }
}
