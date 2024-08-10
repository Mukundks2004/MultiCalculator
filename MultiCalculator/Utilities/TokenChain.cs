using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;
using System.DirectoryServices.ActiveDirectory;
using System.Linq.Expressions;

namespace MultiCalculator.Utilities
{
	public class TokenChain
	{
		public int Cursor { get; private set; }

		List<IButtonOperation> operations;

		public TokenChain()
		{
			operations = [];
		}

		public TokenChain(IEnumerable<IButtonOperation> operations)
		{
			Cursor = operations.Count();
			this.operations = operations.ToList();
		}

		public void Add(IButtonOperation button)
		{
			operations.Insert(Cursor, button);
			Cursor++;
		}

		public void Remove()
		{
			if (!(Cursor == 0))
			{
				operations.RemoveAt(Cursor - 1);
				Cursor--;
			}
		}

		public override string ToString()
		{
			return string.Join(string.Empty, operations.Select(t => t.DisplayName));
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
			var a = HasMatchingAndNonEmptyBraces();
			var b = NumbersExistAndAreWellFormed();
			var c = NoConsecutiveBinaryOperations();
			var d = NoDigitsFollowClosingBrace();

			return HasMatchingAndNonEmptyBraces() && NumbersExistAndAreWellFormed() && NoConsecutiveBinaryOperations() && NoDigitsFollowClosingBrace();
		}

		bool NoDigitsFollowClosingBrace()
		{
			bool currentTokenIsClosingBrace = false;
			foreach (var operation in operations)
			{
				if (operation is BracketButtonOperation bracketButtonOperation && bracketButtonOperation.BracketType == BracketType.Closed || operation is NullaryButtonOperation)
				{
					currentTokenIsClosingBrace = true;
				}
				else
				{
					if (operation is DigitButtonOperation nextDigit)
					{
						if (currentTokenIsClosingBrace)
						{
							return false;
						}
					}
					currentTokenIsClosingBrace = false;
				}
			}

			return true;
		}

		bool NoConsecutiveBinaryOperations()
		{
			bool currentOperationIsBinaryAndFirstBinaryOperationInSequence = false;
			
			if (operations[0] is BinaryButtonOperation firstOperation && !firstOperation.IsUnary)
			{
				return false;
			}

			foreach (var operation in operations)
			{
				if (operation is BinaryButtonOperation binaryOperation)
				{
					if (!currentOperationIsBinaryAndFirstBinaryOperationInSequence)
					{
						currentOperationIsBinaryAndFirstBinaryOperationInSequence = true;
					}
					else
					{
						if (!binaryOperation.IsUnary)
						{
							return false;
						}
					}
				}
				else
				{
					currentOperationIsBinaryAndFirstBinaryOperationInSequence = false;
				}
			}

			return true;
		}

		bool HasMatchingAndNonEmptyBraces()
		{
			var bracketStack = new Stack<int>();

			for (int i = 0; i < operations.Count; i++)
			{
				if (operations[i] is BracketButtonOperation openBracket && openBracket.BracketType == BracketType.Open || operations[i] is UnaryButtonOperation)
				{
					bracketStack.Push(i);
				}
				else if (operations[i] is BracketButtonOperation closedBracket && closedBracket.BracketType == BracketType.Closed)
				{
					if (bracketStack.Count == 0)
					{
						return false;
					}

					int openingIndex = bracketStack.Pop();
					if (i - openingIndex == 1)
					{
						return false;
					}
				}
			}

			return bracketStack.Count >= 0;
		}

		bool NumbersExistAndAreWellFormed()
		{
			bool encounteredDecimalPointInNumber;
			bool atLeastOneIntInNumber;
			bool atLeastOneNumber = false;
			int currentTokenIndex = 0;

			while (currentTokenIndex < operations.Count)
			{
				if (operations[currentTokenIndex] is NullaryButtonOperation)
				{
					atLeastOneNumber = true;
				}
				else if (operations[currentTokenIndex] is DigitButtonOperation)
				{
					atLeastOneNumber = true;
					encounteredDecimalPointInNumber = false;
					atLeastOneIntInNumber = false;
					while (currentTokenIndex < operations.Count && operations[currentTokenIndex] is DigitButtonOperation digit)
					{
						if (digit.DisplayName.Equals("."))
						{
							if (encounteredDecimalPointInNumber)
							{
								return false;
							}

							encounteredDecimalPointInNumber = true;
						}
						else
						{
							atLeastOneIntInNumber = true;
						}

						currentTokenIndex++;
					}

					if (!atLeastOneIntInNumber)
					{
						return false;
					}
				}

				currentTokenIndex++;
			}

			if (!atLeastOneNumber)
			{
				return false;
			}

			return true;
		}
	}
}
