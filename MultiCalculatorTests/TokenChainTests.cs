using MultiCalculator.Implementations;
using MultiCalculator.Utilities;

using static MultiCalculator.Definitions.OperationDefinitions;

namespace MultiCalculatorTests
{
	[TestFixture]
	public class TokenChainTests
	{
		static UnaryOperationToken Factorial { get => MultiCalculator.Definitions.OperationDefinitions.Factorial; }

		static DualArityOperationToken Plus { get => Addition; }

		static DualArityOperationToken Minus { get => Subtraction; }

		static BinaryOperationToken Times { get => Multiplication; }

		static BinaryOperationToken Dividedby { get => Division; }

		static DigitToken One { get => MultiCalculator.Definitions.OperationDefinitions.One; }

		static DigitToken Two { get => MultiCalculator.Definitions.OperationDefinitions.Two; }

		static DigitToken Three { get => MultiCalculator.Definitions.OperationDefinitions.Three; }

		static DigitToken Four { get => MultiCalculator.Definitions.OperationDefinitions.Four; }

		static DigitToken Five { get => MultiCalculator.Definitions.OperationDefinitions.Five; }

		static DigitToken Point { get => MultiCalculator.Definitions.OperationDefinitions.Point; }

		static BracketToken C { get => OpenBracket; }

		static BracketToken J { get => ClosedBracket; }

		static NullaryOperationToken Pi { get => MultiCalculator.Definitions.OperationDefinitions.Pi; }

		static NullaryOperationToken E { get => MultiCalculator.Definitions.OperationDefinitions.E; }

		static UnaryOperationToken Sin { get => MultiCalculator.Definitions.OperationDefinitions.Sin; }

		[Test, TestCaseSource(typeof(TokenChainTests), nameof(ValidAndInvalidExpressionTestCases))]
		public void IsExpressionValidTest(TokenChain tokenChain, bool expectedIsValid)
		{
			Assert.That(tokenChain.IsValid(), Is.EqualTo(expectedIsValid));
		}

		[Test, TestCaseSource(typeof(TokenChainTests), nameof(ParseExpressionTestCases))]
		public void ParseExpressionTest(TokenChain tokenChain, double expectedResult)
		{
			tokenChain.InsertMultiplicationSignsConvertUnaryDualsToUnaryPlaceBrackets();
			Assert.That(tokenChain.Parse(), Is.EqualTo(expectedResult));
		}

		public static IEnumerable<TestCaseData> ValidAndInvalidExpressionTestCases
		{
			get
			{
				yield return new TestCaseData(new TokenChain([One]), true).SetDescription("1");
				yield return new TestCaseData(new TokenChain([One, Two, Three, Four, Five]), true).SetDescription("12345");
				yield return new TestCaseData(new TokenChain([One, Times, Two]), true).SetDescription("1 x 2");
				yield return new TestCaseData(new TokenChain([One, Times, Two, One]), true).SetDescription("1 x 21");
				yield return new TestCaseData(new TokenChain([Two, Two, Point, Two, Times, Two, One, Point, One]), true).SetDescription("22.2 x 21.1");
				yield return new TestCaseData(new TokenChain([One, Point, Five, Point, Two]), false).SetDescription("1.5.2");
				yield return new TestCaseData(new TokenChain([One, Point, Five, Point, Two, Point, One]), false).SetDescription("1.5.2.1");
				yield return new TestCaseData(new TokenChain([Three, Plus, Three, Point, Three, Plus, Three, Point, Three, Point, Three]), false).SetDescription("3 + 3.3 + 3.3.3");
				yield return new TestCaseData(new TokenChain([Two, Two, Point, Two, Times, Two, MultiCalculator.Definitions.OperationDefinitions.One, Point, MultiCalculator.Definitions.OperationDefinitions.One, Point, MultiCalculator.Definitions.OperationDefinitions.One, Plus, MultiCalculator.Definitions.OperationDefinitions.One]), false).SetDescription("22.2 x 21.1.1 + 1");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Times, Two, Times, Three]), true).SetDescription("1 x 2 x 3");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Times, Two, Three, Four, Plus, Five]), true).SetDescription("1 x 234 + 5");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Two, Three, Point, Four, Five]), true).SetDescription("123.45");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Two, Three, Point, Four, Five, Plus, Two, Point, Three]), true).SetDescription("123.45 + 2.3");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Point, Five, Times, Two]), true).SetDescription("1.5 x 2");

				yield return new TestCaseData(new TokenChain([Point]), false).SetDescription(".");
				yield return new TestCaseData(new TokenChain([Point, Plus, Point]), false).SetDescription(". + .");
				yield return new TestCaseData(new TokenChain([Point, Plus, One, Point]), false).SetDescription(". + 1.");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Plus, One, Point]), true).SetDescription("1 + 1.");
				yield return new TestCaseData(new TokenChain([MultiCalculator.Definitions.OperationDefinitions.One, Point, Plus, One, Point]), true).SetDescription("1. + 1.");

				yield return new TestCaseData(new TokenChain([C]), false).SetDescription("(");
				yield return new TestCaseData(new TokenChain([C]), false).SetDescription("(.)");
				yield return new TestCaseData(new TokenChain([J]), false).SetDescription(")");
				yield return new TestCaseData(new TokenChain([C, Three]), true).SetDescription("(3");
				yield return new TestCaseData(new TokenChain([C, Three, J]), true).SetDescription("(3)");
				yield return new TestCaseData(new TokenChain([C, Three, Times, Three]), true).SetDescription("(3 x 3");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Times, Three]), true).SetDescription("(((3 x 3");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, J, Times, Three, J]), true).SetDescription("(((3) x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Times, Three, J]), true).SetDescription("(((3.) x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Point, Times, Three, J]), false).SetDescription("(((3.). x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, Times, Three, J]), false).SetDescription("(((3.)3 x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, J, Times, Three, J]), false).SetDescription("(((3.)3) x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, J, J, Times, Three]), false).SetDescription("(((3.)3)) x 3");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, J, J, Times, Three, Point]), false).SetDescription("(((3.)3)) x 3.");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, J, J, Times, Three, J]), false).SetDescription("(((3.)3)) x 3)");
				yield return new TestCaseData(new TokenChain([C, C, C, Three, Point, J, Three, J, J, Times, Three, J, J]), false).SetDescription("(((3.)3)) x 3))");

				yield return new TestCaseData(new TokenChain([One, Plus, Plus, Two]), true).SetDescription("1 + + 2");
				yield return new TestCaseData(new TokenChain([One, Times, Times, Two]), false).SetDescription("1 x x 2");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Two]), true).SetDescription("1 x + 2");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Plus, Five]), true).SetDescription("1 x + + 5");
				yield return new TestCaseData(new TokenChain([One, Plus, Times, Two]), false).SetDescription("1 + * 2");
				yield return new TestCaseData(new TokenChain([One, Plus, Times, Plus, Two]), false).SetDescription("1 + * + 2");
				yield return new TestCaseData(new TokenChain([One, Plus, Minus, Plus, Two]), true).SetDescription("1 + - + 2");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Two, Plus, Two]), true).SetDescription("1 * + 2 + 2");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Two, Plus, Two, Point, Two, Plus, Plus, Three]), true).SetDescription("1 * + 2 + 2.2 + + 3");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Two, Plus, Two, Point, Two, Plus, Plus, Three, Point, Point]), false).SetDescription("1 * + 2 + 2.2 + + 3..");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Plus, Two]), true).SetDescription("1 * + + 2");
				yield return new TestCaseData(new TokenChain([One, Times, Plus, Minus, Two]), true).SetDescription("1 * + - 2");
				yield return new TestCaseData(new TokenChain([One, Times, Minus, Minus, Two]), true).SetDescription("1 * - - 2");
				yield return new TestCaseData(new TokenChain([One, Times, Minus, Minus, Times, Two]), false).SetDescription("1 * - - * 2");
				yield return new TestCaseData(new TokenChain([One, Plus, Minus, Plus, Minus, Plus, Minus, Two, Point]), true).SetDescription("1 + - + - + - 2.");

				yield return new TestCaseData(new TokenChain([Times, Three]), false).SetDescription("x 3");
				yield return new TestCaseData(new TokenChain([Plus, Three]), true).SetDescription("+ 3");
				yield return new TestCaseData(new TokenChain([Plus, Minus, Three]), true).SetDescription("+ - 3");
				yield return new TestCaseData(new TokenChain([Plus, C, Three, Point]), true).SetDescription("+ (3.");
				yield return new TestCaseData(new TokenChain([Times, C, Three, Point]), false).SetDescription("x (3.");
				yield return new TestCaseData(new TokenChain([C, Three, J, Three]), false).SetDescription("(3)3");
				yield return new TestCaseData(new TokenChain([Three, C, Three, J]), true).SetDescription("3(3)");
				yield return new TestCaseData(new TokenChain([C, Three, J, C, Three, J]), true).SetDescription("(3)(3)");

				yield return new TestCaseData(new TokenChain([Sin, Three]), true).SetDescription("sin 3");
				yield return new TestCaseData(new TokenChain([Sin, J]), false).SetDescription("sin()");
				yield return new TestCaseData(new TokenChain([Sin, Pi, J]), true).SetDescription("sin(pi)");
				yield return new TestCaseData(new TokenChain([Sin, Pi]), true).SetDescription("sin(pi");
				yield return new TestCaseData(new TokenChain([Sin, Point, J]), false).SetDescription("sin(.)");
				yield return new TestCaseData(new TokenChain([Three, Sin, Three]), true).SetDescription("3 sin 3");
				yield return new TestCaseData(new TokenChain([Sin, Three, Three]), true).SetDescription("sin 33");
				yield return new TestCaseData(new TokenChain([Sin, Three, J, Three]), false).SetDescription("sin(3)3");
				yield return new TestCaseData(new TokenChain([Three, Sin, Three, J, Three]), false).SetDescription("3sin(3)3");
				yield return new TestCaseData(new TokenChain([Three, Sin, Three, J, Times, Three]), true).SetDescription("3sin(3) x 3");

				yield return new TestCaseData(new TokenChain([Pi]), true).SetDescription("pi");
				yield return new TestCaseData(new TokenChain([Pi, Pi]), true).SetDescription("pi pi");
				yield return new TestCaseData(new TokenChain([Pi, E]), true).SetDescription("pi e");
				yield return new TestCaseData(new TokenChain([Pi, Times, Pi]), true).SetDescription("pi x pi");
				yield return new TestCaseData(new TokenChain([Pi, Times, Minus, Pi]), true).SetDescription("pi x - pi");
				yield return new TestCaseData(new TokenChain([Pi, Three]), false).SetDescription("pi 3");
				yield return new TestCaseData(new TokenChain([Three, Pi]), true).SetDescription("3 pi");
				yield return new TestCaseData(new TokenChain([C, Pi, J, Pi]), true).SetDescription("(pi)pi");

				yield return new TestCaseData(new TokenChain([C, Two, Plus, Two, J, C, Three, Times, Three]), true).SetDescription("(2 + 2)(3 x 3");
				yield return new TestCaseData(new TokenChain([Two, Times, Dividedby, Three]), false).SetDescription("2 x / 3");
				yield return new TestCaseData(new TokenChain([Two, C]), false).SetDescription("2(");
				yield return new TestCaseData(new TokenChain([Two, Times]), false).SetDescription("2 x");
				yield return new TestCaseData(new TokenChain([Two, Plus]), false).SetDescription("2 +");
				yield return new TestCaseData(new TokenChain([Two, Sin]), false).SetDescription("2 sin");
				yield return new TestCaseData(new TokenChain([Two, Times, Sin]), false).SetDescription("2 x sin");
				yield return new TestCaseData(new TokenChain([Two, Plus, Point]), false).SetDescription("2 + .");
			}
		}

		public static IEnumerable<TestCaseData> ParseExpressionTestCases
		{
			get
			{
				yield return new TestCaseData(new TokenChain([One, Plus, One]), 2).SetDescription("1 + 1 = 2");
				yield return new TestCaseData(new TokenChain([Two, Plus, Three, Times, Three, Times, Three]), 29).SetDescription("2 + 3 x 3 x 3 = 29");
				yield return new TestCaseData(new TokenChain([One, Times, C, Two, Plus, Three, J, Times, Four]), 20).SetDescription("1 x (2 + 3) x 4 = 20");
				yield return new TestCaseData(new TokenChain([One, Plus, Two, Times, Three, Plus, Four]), 11).SetDescription("1 + 2 x 3 + 4 = 11");
				yield return new TestCaseData(new TokenChain([One, Plus, One, Plus, One]), 3).SetDescription("1 + 1 + 1 = 3");
				yield return new TestCaseData(new TokenChain([One, Plus, Plus, One]), 2).SetDescription("1 + + 1 = 2");
				yield return new TestCaseData(new TokenChain([One, Minus, Minus, One]), 2).SetDescription("1 - - 1 = 2");
				yield return new TestCaseData(new TokenChain([One, Minus, Plus, One]), 0).SetDescription("1 - + 1 = 0");
				yield return new TestCaseData(new TokenChain([One, Plus, Minus, One]), 0).SetDescription("1 + - 1 = 0");
				yield return new TestCaseData(new TokenChain([One, Plus, Plus, Plus, One]), 2).SetDescription("1 + + + 1 = 2");
				yield return new TestCaseData(new TokenChain([One]), 1).SetDescription("1 = 1");
				yield return new TestCaseData(new TokenChain([Plus, One]), 1).SetDescription("+1 = 1");
				yield return new TestCaseData(new TokenChain([One, Plus, Two]), 3).SetDescription("1 + 2 = 3");
				yield return new TestCaseData(new TokenChain([One, Plus, Three, Times, Two]), 7).SetDescription("1 + 3 x 2 = 7");
				yield return new TestCaseData(new TokenChain([One, Times, Three, Plus, Two]), 5).SetDescription("1 x 3 + 2 = 5");
				yield return new TestCaseData(new TokenChain([One, Times, Three, Times, Two]), 6).SetDescription("1 x 3 x 2 = 6");
				yield return new TestCaseData(new TokenChain([One, Plus, Three, Plus, Two]), 6).SetDescription("1 + 3 + 2 = 6");
				yield return new TestCaseData(new TokenChain([C, One, Plus, One, J, Times, C, One, Plus, One, J]), 4).SetDescription("(1 + 1) x (1 + 1) = 4");
				yield return new TestCaseData(new TokenChain([C, One, Plus, One, J, Times, C, One, Plus, One]), 4).SetDescription("(1 + 1) x (1 + 1 = 4");

				yield return new TestCaseData(new TokenChain([One, Factorial]), 1).SetDescription("1! = 1");
				yield return new TestCaseData(new TokenChain([C, One, Plus, One, J, Factorial]), 2).SetDescription("(1 + 1)! = 2");
				yield return new TestCaseData(new TokenChain([C, One, Minus, One, J, Factorial]), 1).SetDescription("(1 - 1)! = 1");
				yield return new TestCaseData(new TokenChain([C, One, Plus, One, J, Factorial, Factorial, Factorial]), 2).SetDescription("(1 + 1)!!! = 2");
				yield return new TestCaseData(new TokenChain([Sin, C, One, Minus, One, J]), 0).SetDescription("sin(1 - 1) = 0");
				yield return new TestCaseData(new TokenChain([Minus, C, One, Plus, One, J, Factorial, Factorial, Factorial]), 2).SetDescription("-(1 + 1)!!! = -2");

			}
		}
	}
}