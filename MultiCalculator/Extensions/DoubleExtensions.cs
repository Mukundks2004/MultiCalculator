namespace MultiCalculator.Extensions
{
	public static class DoubleExtensions
	{
		public static bool IsEqualTo(this double a, double b, double tolerance)
		{
			return Math.Abs(a - b) <= tolerance;
		}
	}
}
