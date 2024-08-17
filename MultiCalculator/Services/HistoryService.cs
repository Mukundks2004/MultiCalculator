namespace MultiCalculator.Services
{
	public class HistoryService
	{
		public Stack<string> History = new();

		public void ClearHistory()
		{
			History.Clear();
		}

		public void Push(string value)
		{
			History.Push(value);
		}

		public string Pop()
		{
			return History.Pop();
		}

		public string Peek()
		{
			return History.Peek();
		}
	}
}
