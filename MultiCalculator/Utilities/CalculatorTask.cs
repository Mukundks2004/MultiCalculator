using MultiCalculator.Abstractions;
using MultiCalculator.Delegates;

namespace MultiCalculator.Utilities
{
	public class CalculatorTask
	{
		string name = string.Empty;

		public event SimpleEventHandler? TokensUpdated;

		public CalculatorTask()
		{
			Tokens = [];
		}

		public void Add(IToken token)
		{
			Tokens.Add(token);
		}

		public void Add(IEnumerable<IToken> newTokens)
		{
			foreach (var token in newTokens)
			{
				Tokens.Add(token);
			}

			TokensUpdated?.Invoke();
		}

		public string Name
		{
			get => name;
			set { name = value; TokensUpdated?.Invoke(); }
		}

		public List<IToken> Tokens { get; init; }
	}
}
