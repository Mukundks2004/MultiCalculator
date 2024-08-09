using MultiCalculator.Utilities;
using MultiCalculator.Implementations;
using System.Windows.Controls;
using MultiCalculator.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace MultiCalculatorTests
{
	public class TokenChainTests
	{
		[Test]
		public void Test1()
		{
			var digit1 = new DigitButtonOperation() { DisplayName = "1" };
			var digit2 = new DigitButtonOperation() { DisplayName = "2" };
			var point = new DigitButtonOperation() { DisplayName = "." };
			var add = new BinaryButtonOperation() { DisplayName = "+", Calculate = (a, b) => a + b };

			var tokens = new TokenChain<IButtonOperation>();
			tokens.Add(digit1);
			tokens.Add(add);
			tokens.Add(digit2);

			var tokens2 = new TokenChain<IButtonOperation>();
			tokens2.Add(digit2);
			tokens2.Add(point);
			tokens2.Add(digit2);
			tokens2.Add(digit2);
			tokens2.Add(digit1);
			tokens2.Add(point);

			var tokens3 = new TokenChain<IButtonOperation>();
			tokens3.Add(digit2);
			tokens3.Add(point);
			tokens3.Add(digit2);
			tokens3.Add(digit2);
			tokens3.Add(digit2);
			tokens3.Add(digit1);
			tokens3.Add(digit1);
			tokens3.Add(digit2);
			tokens3.Add(digit1);


			Assert.That(tokens.AllNumbersWellFormed(), Is.True);
			Assert.That(tokens2.AllNumbersWellFormed(), Is.False);
			Assert.That(tokens3.AllNumbersWellFormed(), Is.True);
		}
	}
}