using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;

namespace MultiCalculator.Utilities
{
	public class TokenChain<T> where T : IButtonOperation
	{
		public int Cursor { get; private set; }

		List<IButtonOperation> operations;

		public TokenChain()
		{
			operations = [];
		}

		public void Add(IButtonOperation button)
		{
			operations.Insert(Cursor, button);
			Cursor++;
		}

		public void Remove()
		{
			operations.RemoveAt(Cursor);
			Cursor--;
		}

		//IN THE FUTURE, HAVE A VALIDATOR U CAN USE FOR DERIVATIVES, ETC.
		//THEREFORE THIS VALID FUNCTION WILL BE CALLED ON A _VALIDATOR
		//conditions for validity:
		//cannot have */* (non unary operators MUST be followed by numbers)
		//all brackets matched
		//no pairs of brackets empty
		//unary operators can be followed by unary operators eg + - + 3 is valid
		//separate service to fix the expression called simplify -> but we cannot simplify 
		//2 pairs of brackets without a times symbol should have it inserted eg (2+1)(2+1) = (2+1)*(2+1)
		//Dont forget, 3sin(x) is valid

		//If isValid, make sure to add brakcets, * smbols, etc. in a method afterwards
		public bool IsValid()
		{
			//Check that all pairs of brackets match
			int bracketCount = 0;
			bool previousTokenIsBinaryOperation = false;
			bool previousTokenIsOpenBracket = false;

			foreach (var button in operations)
			{
				if (button is BinaryButtonOperation)
				{
					if (previousTokenIsBinaryOperation)
					{
						return false;
					}

					previousTokenIsBinaryOperation = true;
				}

				previousTokenIsBinaryOperation = false;
				if (button is UnaryButtonOperation)
				{
					previousTokenIsOpenBracket = true;
					bracketCount++;
				}
				
				if (button is BracketButtonOperation bracketButtonOperation)
				{
					if (bracketButtonOperation.BracketType == BracketType.Open)
					{
						previousTokenIsOpenBracket = true;
						bracketCount++;
					}
					else
					{
						if (previousTokenIsOpenBracket)
						{
							return false;
						}
						bracketCount--;
					}
				}

				if (bracketCount < 0)
				{
					return false;
				}
			}
			return true;
		}

		public bool HasMatchingBraces()
		{
			int bracketCount = 0;
			foreach (var operation in operations)
			{
				if (operation is UnaryButtonOperation)
				{
					bracketCount++;
				}

				if (operation is BracketButtonOperation bracketButtonOperation)
				{
					if (bracketButtonOperation.BracketType == BracketType.Open)
					{
						bracketCount++;
					}
					else
					{
						bracketCount--;
					}
				}

				if (bracketCount < 0)
				{
					return false;
				}
			}

			return true;
		}

		public bool AllNumbersWellFormed()
		{
			bool encounteredDecimalPointInNumber;
			for (int i = 0; i < operations.Count; i++)
			{
				if (operations[i] is DigitButtonOperation)
				{
					encounteredDecimalPointInNumber = false;
					while (operations[i] is DigitButtonOperation digit && i < operations.Count)
					{
						if (digit.DisplayName.Equals("."))
						{
							if (encounteredDecimalPointInNumber)
							{
								return false;
							}

							encounteredDecimalPointInNumber = true;
						}

						i++;
					}
				}
			}

			return true;
		}
	}
}
