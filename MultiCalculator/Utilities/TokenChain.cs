using MultiCalculator.Abstractions;
using MultiCalculator.Definitions;
using MultiCalculator.Delegates;
using MultiCalculator.Enums;
using MultiCalculator.Implementations;
using System.Windows.Controls;

namespace MultiCalculator.Utilities
{
	public class TokenChain
	{
		public event SimpleEventHandler? OperationsUpdated;

		List<IToken> operations;

		public int Cursor { get; private set; }

		public TokenChain()
		{
			operations = [];
		}

		public TokenChain(IEnumerable<IToken> operations)
		{
			Cursor = operations.Count();
			this.operations = operations.ToList();
		}

		public override string ToString()
		{
			return string.Join(" ", operations.Select(o => o.TokenSymbol));
		}

		public void Add(IToken button)
		{
			operations.Insert(Cursor, button);
			Cursor++;
			OperationsUpdated?.Invoke();
		}

		public void Add(IEnumerable<IToken> operations)
		{
			foreach (var operation in operations)
			{
				Add(operation);
			}
		}

		public void InsertAt(int index, IToken button)
		{
			operations.Insert(index, button);
			OperationsUpdated?.Invoke();
		}

		public void RemoveLastBeforeCursor()
		{
			if (!(Cursor == 0))
			{
				operations.RemoveAt(Cursor - 1);
				Cursor--;
				OperationsUpdated?.Invoke();
			}
		}

		public void MakeEmpty()
		{
			operations = [];
			Cursor = 0;
			OperationsUpdated?.Invoke();
		}

		public bool IsValid()
		{
			return HasNonEmptyAndNonNegativeOpenBraces() && NumbersExistAndAreWellFormed() && NoConsecutiveBinaryOperations() && NoDigitsFollowClosingBrace() && ExpressionDoesNotEndInOperation();
		}

		public double Parse()
		{
			return ParseFromIndexToIndex(0, operations.Count).Calculate();
		}

		NullaryOperationToken ParseFromIndexToIndex(int startIndex, int EndIndexExclusive)
		{
			var currentIndex = startIndex;
			var numStack = new Stack<NullaryOperationToken>();
			var opStack = new Stack<IBinaryOperation>();
			return new NullaryOperationToken();
		}
		//Still cannot handle 2 x ----4
		//Also not sure if (2 x 2, missing brackets, parses correctly
		/*NullaryOperationToken ParseFromIndexToIndex(int startIndex, int indexEndExclusive)
		{
			var currentIndex = startIndex;
			var numStack = new Stack<NullaryOperationToken>();
			var opStack = new Stack<IBinaryOperation>();

			while (currentIndex < indexEndExclusive)
			{
				if (operations[currentIndex] is NullaryOperationToken nullaryOperation)
				{
					numStack.Push(nullaryOperation);
				}
				else if (operations[currentIndex] is DigitToken)
				{
					var parsedNumberAsNullary = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStack.Push(parsedNumberAsNullary);
					currentIndex += lengthOfNumber - 1;
				}
				else if (operations[currentIndex] is BracketToken openingBracket && openingBracket.BracketType == BracketType.Open)
				{
					int nextClosingBraceDistance = 1;
					int unmatchedOpenBracketCount = 1;
					while (currentIndex + nextClosingBraceDistance < indexEndExclusive && unmatchedOpenBracketCount > 0)
					{
						if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.ClosedBracket)
						{
							unmatchedOpenBracketCount -= 1;
						}
						else if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.OpenBracket || operations[currentIndex + nextClosingBraceDistance] is UnaryOperationToken)
						{
							unmatchedOpenBracketCount += 1;
						}
						nextClosingBraceDistance++;
					}

					numStack.Push(ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance));
					currentIndex += nextClosingBraceDistance;
				}
				else if (operations[currentIndex] is UnaryOperationToken unaryOperation)
				{
					if (unaryOperation.Position == OperandPosition.Postfix)
					{
						var operand = numStack.Pop();
						numStack.Push(NullaryOperationToken.GetConstFromDouble(unaryOperation.CalculateUnary(operand.Calculate()));
					}
					else
					{
						int nextClosingBraceDistance = 1;
						int unmatchedOpenBracketCount = 1;
						while (currentIndex + nextClosingBraceDistance < indexEndExclusive && unmatchedOpenBracketCount > 0)
						{
							if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.ClosedBracket)
							{
								unmatchedOpenBracketCount -= 1;
							}
							else if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.OpenBracket || operations[currentIndex + nextClosingBraceDistance] is UnaryOperationToken)
							{
								unmatchedOpenBracketCount += 1;
							}
							nextClosingBraceDistance++;
						}

						var nullaryResultFromBraces = ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance);
						numStack.Push(new NullaryOperationToken() { Calculate = () => unaryOperation.CalculateUnary(nullaryResultFromBraces.Calculate()) });
						currentIndex += nextClosingBraceDistance;
					}
				}
				else if (operations)
				/*
				//Note: this doesnt properly work haha
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
		}*/

		//Write test cases
		public void InsertMultiplicationSigns()
		{
			IToken currentToken, nextToken;
			for (int i = 0; i < operations.Count; i++)
			{
				currentToken = operations[i];
				nextToken = operations[i + 1];

				if ((currentToken == OperationDefinitions.ClosedBracket || currentToken is NullaryOperationToken or DigitToken) &&
					(nextToken == OperationDefinitions.OpenBracket || nextToken is NullaryOperationToken or UnaryOperationToken))
				{
					InsertAt(i, OperationDefinitions.Multiplication);
				}
			}
		}

		NullaryOperationToken ParseDoubleFromIndex(int index, out int lengthParsed)
		{
			var resultAsString = (operations[index] as DigitToken)!.TokenSymbol;
			index++;
			lengthParsed = 1;

			while (index < operations.Count && operations[index] is DigitToken digit)
			{
				resultAsString += digit.TokenSymbol;
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
				if (operation is BracketToken bracketButtonOperation && bracketButtonOperation.BracketType == BracketType.Closed || operation is NullaryOperationToken)
				{
					currentTokenIsClosingBrace = true;
				}
				else
				{
					if (operation is DigitToken nextDigit)
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

		bool HasNonEmptyAndNonNegativeOpenBraces()
		{
			var bracketStack = new Stack<int>();

			for (int i = 0; i < operations.Count; i++)
			{
				if (operations[i] is BracketToken openBracket && openBracket.BracketType == BracketType.Open || operations[i] is UnaryOperationToken)
				{
					bracketStack.Push(i);
				}
				else if (operations[i] is BracketToken closedBracket && closedBracket.BracketType == BracketType.Closed)
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
			return !(finalToken is BracketToken bracket && bracket.BracketType == BracketType.Open || finalToken is UnaryOperationToken || finalToken is BinaryOperationToken || finalToken is DualArityOperationToken);
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
				else if (operations[currentTokenIndex] is DigitToken)
				{
					atLeastOneNumber = true;
					encounteredDecimalPointInNumber = false;
					atLeastOneIntInNumber = false;
					while (currentTokenIndex < operations.Count && operations[currentTokenIndex] is DigitToken digit)
					{
						if (digit.TokenSymbol.Equals("."))
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
