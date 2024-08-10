using MultiCalculator.Utilities;
using MultiCalculator.Implementations;
using NUnit.Framework.Interfaces;
using MultiCalculator.Abstractions;

namespace MultiCalculatorTests
{
	[TestFixture]
	public class TokenChainTests
	{
		static readonly DigitButtonOperation one = new DigitButtonOperation() { DisplayName = "1" };
		static readonly DigitButtonOperation two = new DigitButtonOperation() { DisplayName = "2" };
		static readonly DigitButtonOperation three = new DigitButtonOperation() { DisplayName = "3" };
		static readonly DigitButtonOperation four = new DigitButtonOperation() { DisplayName = "4" };
		static readonly DigitButtonOperation five = new DigitButtonOperation() { DisplayName = "5" };
		static readonly DigitButtonOperation point = new DigitButtonOperation() { DisplayName = "." };

		static readonly BinaryButtonOperation plus = new BinaryButtonOperation() { DisplayName = "+", Calculate = (a, b) => a + b, IsUnary = true };
		static readonly BinaryButtonOperation minus = new BinaryButtonOperation() { DisplayName = "-", Calculate = (a, b) => a - b, IsUnary = true };
		static readonly BinaryButtonOperation times = new BinaryButtonOperation() { DisplayName = "*", Calculate = (a, b) => a * b, IsUnary = false };

		static readonly BracketButtonOperation c = new BracketButtonOperation() { DisplayName = "(", BracketType = BracketType.Open };
		static readonly BracketButtonOperation J = new BracketButtonOperation() { DisplayName = ")", BracketType = BracketType.Closed };


		[Test, TestCaseSource(typeof(TokenChainTests), nameof(ValidAndInvalidExpressionTestCases))]
		public void IsExpressionValid(TokenChain tokenChain, bool expectedIsValid)
		{
			Assert.That(tokenChain.IsValid(), Is.EqualTo(expectedIsValid));
		}

		public static IEnumerable<TestCaseData> ValidAndInvalidExpressionTestCases
		{
			get
			{
				yield return new TestCaseData(new TokenChain([one]), true).SetDescription("1");
				yield return new TestCaseData(new TokenChain([one, two, three, four, five]), true).SetDescription("12345");
				yield return new TestCaseData(new TokenChain([one, times, two]), true).SetDescription("1 x 2");
				yield return new TestCaseData(new TokenChain([one, times, two, one]), true).SetDescription("1 x 21");
				yield return new TestCaseData(new TokenChain([two, two, point, two, times, two, one, point, one]), true).SetDescription("22.2 x 21.1");
				yield return new TestCaseData(new TokenChain([one, point, five, point, two]), false).SetDescription("1.5.2");
				yield return new TestCaseData(new TokenChain([one, point, five, point, two, point, one]), false).SetDescription("1.5.2.1");
				yield return new TestCaseData(new TokenChain([three, plus, three, point, three, plus, three, point, three, point, three]), false).SetDescription("3 + 3.3 + 3.3.3");
				yield return new TestCaseData(new TokenChain([two, two, point, two, times, two, one, point, one, point, one, plus, one]), false).SetDescription("22.2 x 21.1.1 + 1");
				yield return new TestCaseData(new TokenChain([one, times, two, times, three]), true).SetDescription("1 x 2 x 3");
				yield return new TestCaseData(new TokenChain([one, times, two, three, four, plus, five]), true).SetDescription("1 x 234 + 5");
				yield return new TestCaseData(new TokenChain([one, two, three, point, four, five]), true).SetDescription("123.45");
				yield return new TestCaseData(new TokenChain([one, two, three, point, four, five, plus, two, point, three]), true).SetDescription("123.45 + 2.3");
				yield return new TestCaseData(new TokenChain([one, point, five, times, two]), true).SetDescription("1.5 x 2");

				yield return new TestCaseData(new TokenChain([point]), false).SetDescription(".");
				yield return new TestCaseData(new TokenChain([point, plus, point]), false).SetDescription(". + .");
				yield return new TestCaseData(new TokenChain([point, plus, one, point]), false).SetDescription(". + 1.");
				yield return new TestCaseData(new TokenChain([one, plus, one, point]), true).SetDescription("1 + 1.");
				yield return new TestCaseData(new TokenChain([one, point, plus, one, point]), true).SetDescription("1. + 1.");

				yield return new TestCaseData(new TokenChain([one, plus, plus, two]), true).SetDescription("1 + + 2");
				yield return new TestCaseData(new TokenChain([one, times, plus, two]), true).SetDescription("1 * + 2");
				yield return new TestCaseData(new TokenChain([one, times, plus, two, plus, two]), true).SetDescription("1 * + 2 + 2");
				yield return new TestCaseData(new TokenChain([one, times, plus, two, plus, two, point, two, plus, plus, three]), true).SetDescription("1 * + 2 + 2.2 + + 3");
				yield return new TestCaseData(new TokenChain([one, times, plus, two, plus, two, point, two, plus, plus, three, point, point]), false).SetDescription("1 * + 2 + 2.2 + + 3..");
				yield return new TestCaseData(new TokenChain([one, times, plus, plus, two]), true).SetDescription("1 * + + 2");
				yield return new TestCaseData(new TokenChain([one, times, plus, minus, two]), true).SetDescription("1 * + - 2");
				yield return new TestCaseData(new TokenChain([one, times, minus, minus, two]), true).SetDescription("1 * - - 2");
				yield return new TestCaseData(new TokenChain([one, plus, minus, plus, minus, plus, minus, two, point]), true).SetDescription("1 + - + - + - 2.");

				yield return new TestCaseData(new TokenChain([c]), false).SetDescription("(");
				yield return new TestCaseData(new TokenChain([c]), false).SetDescription("(.)");
				yield return new TestCaseData(new TokenChain([J]), false).SetDescription(")");
				yield return new TestCaseData(new TokenChain([c, three]), true).SetDescription("(3");
				yield return new TestCaseData(new TokenChain([c, three, J]), true).SetDescription("(3)");
				yield return new TestCaseData(new TokenChain([c, three, times, three]), true).SetDescription("(3 x 3");
				yield return new TestCaseData(new TokenChain([c, c, c, three, times, three]), true).SetDescription("(((3 x 3");
				yield return new TestCaseData(new TokenChain([c, c, c, three, J, times, three, J]), true).SetDescription("(((3) x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, times, three, J]), true).SetDescription("(((3.) x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, point, times, three, J]), false).SetDescription("(((3.). x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, times, three, J]), true).SetDescription("(((3.)3 x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, J, times, three, J]), true).SetDescription("(((3.)3) x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, J, J, times, three]), true).SetDescription("(((3.)3)) x 3");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, J, J, times, three, point]), true).SetDescription("(((3.)3)) x 3.");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, J, J, times, three, J]), false).SetDescription("(((3.)3)) x 3)");
				yield return new TestCaseData(new TokenChain([c, c, c, three, point, J, three, J, J, times, three, J, J]), false).SetDescription("(((3.)3)) x 3))");
			}
		}
	}
}