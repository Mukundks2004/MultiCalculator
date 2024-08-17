using MultiCalculator.Enums;
using MultiCalculator.Helpers;
using MultiCalculator.Implementations;

namespace MultiCalculator.Definitions
{
    public static class OperationDefinitions
    {
		static readonly DigitToken one = new() { TokenSymbol = "1" };
		static readonly DigitToken two = new() { TokenSymbol = "2" };
		static readonly DigitToken three = new() { TokenSymbol = "3" };
		static readonly DigitToken four = new() { TokenSymbol = "4" };
		static readonly DigitToken five = new() { TokenSymbol = "5" };
		static readonly DigitToken six = new() { TokenSymbol = "6" };
		static readonly DigitToken seven = new() { TokenSymbol = "7" };
		static readonly DigitToken eight = new() { TokenSymbol = "8" };
		static readonly DigitToken nine = new() { TokenSymbol = "9" };
		static readonly DigitToken zero = new() { TokenSymbol = "0" };
		static readonly DigitToken point = new() { TokenSymbol = "." };

		static readonly NullaryOperationToken pi = new() { Calculate = () => Math.PI, TokenSymbol = "π", LatexString = "{\\pi}" };
		static readonly NullaryOperationToken e = new() { Calculate = () => Math.E, TokenSymbol = "e", LatexString = "e" };
		static readonly NullaryOperationToken lemniscate = new() { Calculate = () => 2.6220575542921198, TokenSymbol = "ϖ", LatexString = "{\\varpi}" };
		static readonly NullaryOperationToken mascheroni = new() { Calculate = () => 0.5772156649015329, TokenSymbol = "γ", LatexString = "{\\gamma}" };
		static readonly NullaryOperationToken phi = new() { Calculate = () => (1 + Math.Sqrt(5)) / 2, TokenSymbol = "φ", LatexString = "{\\phi}" };

		//Mukund: fix associativity, precedence for all of these please
		static readonly UnaryOperationToken factorial = new() { CalculateUnary = MathHelpers.Factorial, Fixity = Fixity.Postfix, TokenSymbol = "!", Priority = 6, LatexString = (x) => "{" + x + "}!" };

		static readonly BinaryOperationToken exponentiation = new() { CalculateBinary = Math.Pow, Associativity = Associativity.Right, Priority = 5, TokenSymbol = "^", LatexString = (x, y) => "{" + x + "}^{" + y + "}"};
		static readonly UnaryOperationToken antilog = new() { CalculateUnary = (x) => Math.Pow(10, x), Fixity = Fixity.Prefix, TokenSymbol = "10^", Priority = 5, LatexString = (x) => "10^{" + x + "}" };

		//Requires special attention
		static readonly BinaryOperationToken nthroot = new() { CalculateBinary = (a, b) => Math.Pow(b, 1 / a), Associativity = Associativity.Right, Priority = 5, TokenSymbol = "√", LatexString = (x, y) => "\\sqrt[" + x + "]{" + y + "}" };

		static readonly UnaryOperationToken p = new() { CalculateUnary = (x) => x, Fixity = Fixity.Prefix, TokenSymbol = "+", Priority = 4 , LatexString = (x) => "+{" + x + "}" };
		static readonly UnaryOperationToken u = new() { CalculateUnary = (x) => -x, Fixity = Fixity.Prefix, TokenSymbol = "-", Priority = 4, LatexString = (x) => "-{" + x + "}" };

		static readonly BinaryOperationToken permutations = new() { CalculateBinary = MathHelpers.P, Associativity = Associativity.Left, Priority = 3, TokenSymbol = "P", LatexString = (x, y) => "{" + x + "}P{" + y + "}" };

		static readonly BinaryOperationToken combinations = new() { CalculateBinary = MathHelpers.C, Associativity = Associativity.Left, Priority = 3, TokenSymbol = "C", LatexString = (x, y) => "{" + x + "}C{" + y + "}" };

		static readonly UnaryOperationToken sin = new() { CalculateUnary = Math.Sin, Fixity = Fixity.Prefix, TokenSymbol = "sin", Priority = 2, LatexString = (x) => "\\sin{" + x + "}" };
		static readonly UnaryOperationToken cos = new() { CalculateUnary = Math.Cos, Fixity = Fixity.Prefix, TokenSymbol = "cos", Priority = 2, LatexString = (x) => "\\cos{" + x + "}" };
		static readonly UnaryOperationToken tan = new() { CalculateUnary = Math.Tan, Fixity = Fixity.Prefix, TokenSymbol = "tan", Priority = 2, LatexString = (x) => "\\tan{" + x + "}" };
		static readonly UnaryOperationToken asin = new() { CalculateUnary = Math.Asin, Fixity = Fixity.Prefix, TokenSymbol = "asin", Priority = 2, LatexString = (x) => "\\sin^{-1}{" + x + "}" };
		static readonly UnaryOperationToken acos = new() { CalculateUnary = Math.Acos, Fixity = Fixity.Prefix, TokenSymbol = "acos", Priority = 2, LatexString = (x) => "\\cos^{-1}{" + x + "}" };
		static readonly UnaryOperationToken atan = new() { CalculateUnary = Math.Atan, Fixity = Fixity.Prefix, TokenSymbol = "atan", Priority = 2, LatexString = (x) => "\\tan^{-1}{" + x + "}" };

		static readonly UnaryOperationToken sinh = new() { CalculateUnary = Math.Sinh, Fixity = Fixity.Prefix, TokenSymbol = "sinh", Priority = 2, LatexString = (x) => "\\sinh{" + x + "}" };
		static readonly UnaryOperationToken cosh = new() { CalculateUnary = Math.Cosh, Fixity = Fixity.Prefix, TokenSymbol = "cosh", Priority = 2, LatexString = (x) => "\\cosh{" + x + "}" };
		static readonly UnaryOperationToken tanh = new() { CalculateUnary = Math.Tanh, Fixity = Fixity.Prefix, TokenSymbol = "tanh", Priority = 2, LatexString = (x) => "\\tanh{" + x + "}" };
		static readonly UnaryOperationToken asinh = new() { CalculateUnary = Math.Asinh, Fixity = Fixity.Prefix, TokenSymbol = "asinh", Priority = 2, LatexString = (x) => "\\sinh^{-1}{" + x + "}" };
		static readonly UnaryOperationToken acosh = new() { CalculateUnary = Math.Acosh, Fixity = Fixity.Prefix, TokenSymbol = "acosh", Priority = 2, LatexString = (x) => "\\cosh^{-1}{" + x + "}" };
		static readonly UnaryOperationToken atanh = new() { CalculateUnary = Math.Atanh, Fixity = Fixity.Prefix, TokenSymbol = "atanh", Priority = 2, LatexString = (x) => "\\tanh^{-1}{" + x + "}" };

		static readonly UnaryOperationToken abs = new() { CalculateUnary = Math.Abs, Fixity = Fixity.Prefix, TokenSymbol = "abs", Priority = 2, LatexString = (x) => "abs{" + x + "}" };
		static readonly UnaryOperationToken sqrt = new() { CalculateUnary = Math.Sqrt, Fixity = Fixity.Prefix, TokenSymbol = "sqrt", Priority = 2, LatexString = (x) => "\\sqrt" + x + "}" };
		static readonly UnaryOperationToken cbrt = new() { CalculateUnary = Math.Cbrt, Fixity = Fixity.Prefix, TokenSymbol = "cbrt", Priority = 2, LatexString = (x) => "\\surd[3]{" + x + "}" };
		static readonly UnaryOperationToken log = new() { CalculateUnary = Math.Log10, Fixity = Fixity.Prefix, TokenSymbol = "log", Priority = 2, LatexString = (x) => "\\log{" + x + "}" };
		static readonly UnaryOperationToken ln = new() { CalculateUnary = Math.Log, Fixity = Fixity.Prefix, TokenSymbol = "ln", Priority = 2, LatexString = (x) => "\\ln{" + x + "}" };
		static readonly UnaryOperationToken productlog = new() { CalculateUnary = MathHelpers.LambertW, Fixity = Fixity.Prefix, TokenSymbol = "W", Priority = 2, LatexString = (x) => "W{" + x + "}" };
		static readonly UnaryOperationToken sinc = new() { CalculateUnary = (x) => x == 0 ? double.NaN : Math.Sin(x) / x, Fixity = Fixity.Prefix, TokenSymbol = "sinc", Priority = 2, LatexString = (x) => "sinc{" + x + "}" };
		static readonly UnaryOperationToken exp = new() { CalculateUnary = Math.Exp, Fixity = Fixity.Prefix, TokenSymbol = "exp", Priority = 2, LatexString = (x) => "\\exp{" + x + "}" };
		static readonly UnaryOperationToken erf = new() { CalculateUnary = MathHelpers.Erf, Fixity = Fixity.Prefix, TokenSymbol = "erf", Priority = 2, LatexString = (x) => "\\erf{" + x + "}" };

		static readonly BinaryOperationToken multiplication = new() { CalculateBinary = (a, b) => a * b, Associativity = Associativity.Left, Priority = 1, TokenSymbol = "×", LatexString = (x, y) => "{" + x + "}\\times{" + y + "}" };
        static readonly BinaryOperationToken division = new() { CalculateBinary = (a, b) => b == 0 ? double.NaN : a / b, Associativity = Associativity.Left, Priority = 1, TokenSymbol = "÷", LatexString = (x, y) => "\\frac{" + x + "}{" + y + "}" };

		static readonly DualArityOperationToken addition = new() { CalculateBinary = (a, b) => a + b, Associativity = Associativity.Left, Priority = 0, TokenSymbol = "+", UnaryOperation = p, LatexString = (x, y) => "{" + x + "}+{" + y + "}" };
		static readonly DualArityOperationToken subtraction = new() { CalculateBinary = (a, b) => a - b, Associativity = Associativity.Left, Priority = 0, TokenSymbol = "-", UnaryOperation = u, LatexString = (x, y) => "{" + x + "}-{" + y + "}" };

		public static DualArityOperationToken Addition => addition;

        public static DualArityOperationToken Subtraction => subtraction;

        public static BinaryOperationToken Multiplication => multiplication;

        public static BinaryOperationToken Division => division;

        public static BinaryOperationToken Exponentiation => exponentiation;

        public static BinaryOperationToken Nthroot => nthroot;

        public static BinaryOperationToken Permuations => permutations;

        public static BinaryOperationToken Combinations => combinations;

        public static DigitToken One => one;

        public static DigitToken Two => two;

        public static DigitToken Three => three;

        public static DigitToken Four => four;

        public static DigitToken Five => five;

        public static DigitToken Six => six;

        public static DigitToken Seven => seven;

        public static DigitToken Eight => eight;

        public static DigitToken Nine => nine;

        public static DigitToken Zero => zero;

        public static DigitToken Point => point;

        public static UnaryOperationToken Sin => sin;

        public static UnaryOperationToken Cos => cos;

        public static UnaryOperationToken Tan => tan;

		public static UnaryOperationToken Asin => asin;

		public static UnaryOperationToken Acos => acos;

		public static UnaryOperationToken Atan => atan;

		public static UnaryOperationToken Sinh => sinh;

		public static UnaryOperationToken Cosh => cosh;

		public static UnaryOperationToken Tanh => tanh;

		public static UnaryOperationToken Asinh => asinh;

		public static UnaryOperationToken Acosh => acosh;

		public static UnaryOperationToken Atanh => atanh;

		public static UnaryOperationToken Abs => abs;

		public static UnaryOperationToken Factorial => factorial;

		public static UnaryOperationToken Sqrt => sqrt;

		public static UnaryOperationToken Cbrt => cbrt;

		public static UnaryOperationToken Log => log;

		public static UnaryOperationToken Ln => ln;

		public static UnaryOperationToken Antilog => antilog;

		public static UnaryOperationToken Exp => exp;

		public static UnaryOperationToken Erf => erf;

		public static UnaryOperationToken Productlog => productlog;

		public static UnaryOperationToken Sinc => sinc;

		public static BracketToken ClosedBracket => BracketToken.ClosedBracket;

        public static BracketToken OpenBracket => BracketToken.OpenBracket;

        public static NullaryOperationToken Pi => pi;

        public static NullaryOperationToken E => e;

		public static NullaryOperationToken Lemniscate => lemniscate;

		public static NullaryOperationToken Mascheroni => mascheroni;

		public static NullaryOperationToken Phi => phi;
	}
}
