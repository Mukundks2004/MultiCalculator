namespace MultiCalculator.Helpers
{
    static class MathHelpers
    {
        public static double Factorial(double n)
        {
            if (n % 1 != 0 || n < 0)
            {
                return double.NaN;
            }

            if (n == 0)
            {
                return 1;
            }

            return n * Factorial(n - 1);
        }

        public static double P(double n, double r)
        {
            return Factorial(n) / Factorial(n - r);
        }

		public static double C(double n, double r)
		{
            return P(n, r) / Factorial(r);
		}

		//https://www.johndcook.com/csharp_erf.html
		public static double Erf(double x)
		{
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;

			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Math.Abs(x);

			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

			return sign * y;
		}

		//Halley's method via eqn (5.9) in Corless et al (1996)
		//https://link.springer.com/article/10.1007/BF02124750
		public static double LambertW(double x)
		{
			if (x < -Math.Exp(-1))
				return double.NaN;

			int amountOfIterations = Math.Max(4, (int)Math.Ceiling(Math.Log10(x) / 3));
			double w = 3 * Math.Log(x + 1) / 4;
			for (int i = 0; i < amountOfIterations; i++)
				w -= (w * Math.Exp(w) - x) / (Math.Exp(w) * (w + 1) - (w + 2) * (w * Math.Exp(w) - x) / (2 * w + 2));

			return w;
		}
	}
}
