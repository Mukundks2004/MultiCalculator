using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;

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

		public bool IsValid()
		{
			return HasMatchingAndNonEmptyBraces() && NumbersExistAndAreWellFormed() && NoConsecutiveBinaryOperations() && NoDigitsFollowClosingBrace();
		}

		public double Parse()
		{
			return ParseFromIndexToIndex(0, operations.Count).Calculate();
		}

		//Currently cannot handle 2 x ----4
		NullaryButtonOperation ParseFromIndexToIndex(int startIndex, int indexEndExclusive)
		{
			var currentIndex = startIndex;
			//digits and constants are both nullaries
			var numStack = new Stack<NullaryButtonOperation>();

			//brackets and unaries trigger a recursive call
			//operations are binary
			var opStack = new Stack<BinaryButtonOperation>();

			while (currentIndex < indexEndExclusive)
			{
				if (operations[currentIndex] is NullaryButtonOperation nullaryOperation)
				{
					numStack.Push(nullaryOperation);
				}
				else if (operations[currentIndex] is DigitButtonOperation)
				{
					var parsedNumberAsNullary = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStack.Push(parsedNumberAsNullary);
					currentIndex += lengthOfNumber - 1;
				}
				//we shouldnt ever reach a closing brace but whatever
				else if (operations[currentIndex] is BracketButtonOperation openingBracket && openingBracket.BracketType == BracketType.Open)
				{
					int nextClosingBraceDistance = 1;
					while (currentIndex + nextClosingBraceDistance < indexEndExclusive && !(operations[currentIndex + nextClosingBraceDistance] is BracketButtonOperation bracket && bracket.BracketType == BracketType.Closed))
					{
						nextClosingBraceDistance++;
					}

					numStack.Push(ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance));
					currentIndex += nextClosingBraceDistance;
				}
				else if (operations[currentIndex] is UnaryButtonOperation unaryOperation)
				{
					int nextClosingBraceDistance = 1;
					while (currentIndex + nextClosingBraceDistance < indexEndExclusive && !(operations[currentIndex + nextClosingBraceDistance] is BracketButtonOperation bracket && bracket.BracketType == BracketType.Closed))
					{
						nextClosingBraceDistance++;
					}

					var nullaryResultFromBraces = ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance);
					numStack.Push(new NullaryButtonOperation() { Calculate = () => unaryOperation.Calculate(nullaryResultFromBraces.Calculate()) });
					currentIndex += nextClosingBraceDistance;
				}
				else if (operations[currentIndex] is BinaryButtonOperation binaryOperation)
				{
					if (opStack.Count == 0)
					{
						opStack.Push(binaryOperation);
					}
                    else
                    {
						var mostRecentOperation = opStack.Peek();

						//Having two operators with the same precedence but different associativity would create ambiguity in expressions (impossible)
						//An expression like a op1 b op2 c would have conflicting evaluation orders
						if (mostRecentOperation.Priority == binaryOperation.Priority && mostRecentOperation.Associativity == Associativity.Left || mostRecentOperation.Priority > binaryOperation.Priority)
						{
							var firstOperand = numStack.Pop();
							var secondOperand = numStack.Pop();
							opStack.Pop();
							var result = mostRecentOperation.Calculate(firstOperand.Calculate(), secondOperand.Calculate());
							numStack.Push(new NullaryButtonOperation() { Calculate = () => result });
							opStack.Push(binaryOperation);
						}
						else
						{
							opStack.Push(binaryOperation);
						}
                    }
				}

				currentIndex++;
			}

			while (opStack.Count > 0)
			{
				var secondOperand = numStack.Pop();
				var firstOperand = numStack.Pop();
				var result = opStack.Pop().Calculate(firstOperand.Calculate(), secondOperand.Calculate());
				numStack.Push(new NullaryButtonOperation() { Calculate = () => result });
			}

			return numStack.Pop();
		}

		NullaryButtonOperation ParseDoubleFromIndex(int index, out int lengthParsed)
		{
			var resultAsString = (operations[index] as DigitButtonOperation)!.DisplayName;
			index++;
			lengthParsed = 1;

			while (index < operations.Count && operations[index] is DigitButtonOperation digit)
			{
				resultAsString += digit.DisplayName;
				lengthParsed++;
			}

			var result = double.Parse(resultAsString);
			return new NullaryButtonOperation() { Calculate = () => result };
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
