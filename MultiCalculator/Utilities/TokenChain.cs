using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Implementations;

namespace MultiCalculator.Utilities
{
	public class TokenChain
	{
		public int Cursor { get; private set; }

		List<IToken> operations;

		public TokenChain()
		{
			operations = [];
		}

		public TokenChain(IEnumerable<IToken> operations)
		{
			Cursor = operations.Count();
			this.operations = operations.ToList();
		}

		public void Add(IToken button)
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
			var a = HasMatchingAndNonEmptyBraces();
			var b = NumbersExistAndAreWellFormed();
			var c = NoConsecutiveBinaryOperations();
			var d = NoDigitsFollowClosingBrace();
			var e = ExpressionDoesNotEndInOperation();

			return HasMatchingAndNonEmptyBraces() && NumbersExistAndAreWellFormed() && NoConsecutiveBinaryOperations() && NoDigitsFollowClosingBrace() && ExpressionDoesNotEndInOperation();
		}

		public double Parse()
		{
			return ParseFromIndexToIndex(0, operations.Count).Calculate();
		}

		//Still cannot handle 2 x ----4
		//Also not sure if (2 x 2, missing brackets, parses correctly
		NullaryOperationToken ParseFromIndexToIndex(int startIndex, int indexEndExclusive)
		{
			var currentIndex = startIndex;
			var lastTokenWasStrictlyBinary = false;
			var numStack = new Stack<NullaryOperationToken>();
			var opStack = new Stack<IBinaryOperation>();

			while (currentIndex < indexEndExclusive)
			{
				if (operations[currentIndex] is NullaryOperationToken nullaryOperation)
				{
					numStack.Push(nullaryOperation);
				}
				else if (operations[currentIndex] is DigitOperationToken)
				{
					var parsedNumberAsNullary = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStack.Push(parsedNumberAsNullary);
					currentIndex += lengthOfNumber - 1;
				}
				//we shouldnt ever reach a closing brace but whatever
				else if (operations[currentIndex] is BracketOperationToken openingBracket && openingBracket.BracketType == BracketType.Open)
				{
					int nextClosingBraceDistance = 1;
					while (currentIndex + nextClosingBraceDistance < indexEndExclusive && !(operations[currentIndex + nextClosingBraceDistance] is BracketOperationToken bracket && bracket.BracketType == BracketType.Closed))
					{
						nextClosingBraceDistance++;
					}

					numStack.Push(ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance));
					currentIndex += nextClosingBraceDistance;
				}
				else if (operations[currentIndex] is UnaryOperationToken unaryOperation)
				{
					int nextClosingBraceDistance = 1;
					while (currentIndex + nextClosingBraceDistance < indexEndExclusive && !(operations[currentIndex + nextClosingBraceDistance] is BracketOperationToken bracket && bracket.BracketType == BracketType.Closed))
					{
						nextClosingBraceDistance++;
					}

					var nullaryResultFromBraces = ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance);
					numStack.Push(new NullaryOperationToken() { Calculate = () => unaryOperation.CalculateUnary(nullaryResultFromBraces.Calculate()) });
					currentIndex += nextClosingBraceDistance;
				}
				else if (operations[currentIndex] is IBinaryOperation binaryOperation)
				{
					//Do a quick lookahead to eliminate + and - unaries
					var unaryScanningIndex = 1;
					Func<double, double> unaryDelegate = (x) => x;
					var unaryStack = new Stack<Func<double, double>>();

					while (currentIndex + unaryScanningIndex < operations.Count && operations[currentIndex + unaryScanningIndex] is DualArityOperationToken looseBinaryOperation)
					{
						unaryScanningIndex++;
						unaryStack.Push(looseBinaryOperation.CalculateUnary);
					}

					//Double check this logic
					while (unaryStack.Count > 0)
					{
						var innerFunction = unaryStack.Pop();
						unaryDelegate = (x) => innerFunction(unaryDelegate(x));
					}

					//For the above code: the FIRST operation is always the one to take. in fact, it is the only one we really care about
					//sometimes ya gotta look through the negatives to apply them properly

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
							var result = mostRecentOperation.CalculateBinary(firstOperand.Calculate(), secondOperand.Calculate());
							numStack.Push(new NullaryOperationToken() { Calculate = () => result });
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
				var result = opStack.Pop().CalculateBinary(firstOperand.Calculate(), secondOperand.Calculate());
				numStack.Push(new NullaryOperationToken() { Calculate = () => result });
			}

			return numStack.Pop();
		}

		NullaryOperationToken ParseDoubleFromIndex(int index, out int lengthParsed)
		{
			var resultAsString = (operations[index] as DigitOperationToken)!.DisplayName;
			index++;
			lengthParsed = 1;

			while (index < operations.Count && operations[index] is DigitOperationToken digit)
			{
				resultAsString += digit.DisplayName;
				lengthParsed++;
			}

			var result = double.Parse(resultAsString);
			return new NullaryOperationToken() { Calculate = () => result };
		}

		bool NoDigitsFollowClosingBrace()
		{
			bool currentTokenIsClosingBrace = false;
			foreach (var operation in operations)
			{
				if (operation is BracketOperationToken bracketButtonOperation && bracketButtonOperation.BracketType == BracketType.Closed || operation is NullaryOperationToken)
				{
					currentTokenIsClosingBrace = true;
				}
				else
				{
					if (operation is DigitOperationToken nextDigit)
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
			bool currentOperationIsBinary = true;

			foreach (var operation in operations)
			{
				if (operation is BinaryOperationToken && currentOperationIsBinary)
				{
					return false;
				}

				if (operation is IBinaryOperation)
				{
					currentOperationIsBinary = true;
				}
				else
				{
					currentOperationIsBinary = false;
				}
			}

			return true;
		}

		bool HasMatchingAndNonEmptyBraces()
		{
			var bracketStack = new Stack<int>();

			for (int i = 0; i < operations.Count; i++)
			{
				if (operations[i] is BracketOperationToken openBracket && openBracket.BracketType == BracketType.Open || operations[i] is UnaryOperationToken)
				{
					bracketStack.Push(i);
				}
				else if (operations[i] is BracketOperationToken closedBracket && closedBracket.BracketType == BracketType.Closed)
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

		bool ExpressionDoesNotEndInOperation()
		{
			var finalToken = operations.Last();
			return !(finalToken is BracketOperationToken bracket && bracket.BracketType == BracketType.Open || finalToken is UnaryOperationToken || finalToken is BinaryOperationToken || finalToken is DualArityOperationToken);
		}

		bool NumbersExistAndAreWellFormed()
		{
			bool encounteredDecimalPointInNumber;
			bool atLeastOneIntInNumber;
			bool atLeastOneNumber = false;
			int currentTokenIndex = 0;

			while (currentTokenIndex < operations.Count)
			{
				if (operations[currentTokenIndex] is NullaryOperationToken)
				{
					atLeastOneNumber = true;
				}
				else if (operations[currentTokenIndex] is DigitOperationToken)
				{
					atLeastOneNumber = true;
					encounteredDecimalPointInNumber = false;
					atLeastOneIntInNumber = false;
					while (currentTokenIndex < operations.Count && operations[currentTokenIndex] is DigitOperationToken digit)
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
