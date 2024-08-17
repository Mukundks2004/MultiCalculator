using MultiCalculator.Abstractions;
using MultiCalculator.Definitions;
using MultiCalculator.Delegates;
using MultiCalculator.Enums;
using MultiCalculator.Implementations;

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
			var joinedExpression = string.Join("", operations.Select(o => o.TokenSymbol));
			return joinedExpression.Insert(Cursor, "|");
		}

		public void MoveCursorLeft()
		{
			if (Cursor > 0)
			{
				Cursor--;
				OperationsUpdated?.Invoke();
			}
		}

		public void MoveCursorRight()
		{
			if (Cursor < operations.Count)
			{
				Cursor++;
				OperationsUpdated?.Invoke();
			}
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
			if (index < Cursor)
			{
				Cursor++;
			}
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

		NullaryOperationToken ParseFromIndexToIndex(int startIndex, int endIndexExclusive)
		{
			var currentIndex = startIndex;
			var numStack = new Stack<NullaryOperationToken>();
			var opStack = new Stack<IOperation>();

			while (currentIndex < endIndexExclusive)
			{
				var currentToken = operations[currentIndex];

				if (currentToken is NullaryOperationToken constant)
				{
					numStack.Push(constant);
				}

				else if (currentToken is DigitToken)
				{
					var parsedNumberAsConst = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStack.Push(parsedNumberAsConst);
					currentIndex += lengthOfNumber - 1;
				}

				else if (currentToken == OperationDefinitions.OpenBracket)
				{
					var nextClosingBraceDistance = 1;
					var unmatchedOpenBracketCount = 1;
					while (unmatchedOpenBracketCount > 0)
					{
						if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.ClosedBracket)
						{
							unmatchedOpenBracketCount--;
						}
						else if (operations[currentIndex + nextClosingBraceDistance] == OperationDefinitions.OpenBracket)
						{
							unmatchedOpenBracketCount++;
						}
						nextClosingBraceDistance++;
					}

					numStack.Push(ParseFromIndexToIndex(currentIndex + 1, currentIndex + nextClosingBraceDistance - 1));
					currentIndex += nextClosingBraceDistance - 1;
				}

				//We can never evaluate unaries until we encounter postfix operators or binaries
				//This system should hopefully allow duals to be postfix
				else if (currentToken is UnaryOperationToken unaryPrefix && unaryPrefix.Fixity == Fixity.Prefix)
				{
					opStack.Push(unaryPrefix);
				}

				else if (currentToken is IOperation operation)
				{
					//First operation is not postfix
					if (opStack.Count == 0 && !(operation is UnaryOperationToken firstOperation && firstOperation.Fixity == Fixity.Postfix))
					{
						opStack.Push(operation);
					}
					//only operation is postfix
					else if (opStack.Count == 0)
					{
						var unaryOperation = operation as UnaryOperationToken;
						var unaryOperand = numStack.Pop();
						var result = unaryOperation!.CalculateUnary(unaryOperand.Calculate());
						numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
					}

					//for a second, lets live in a world without dual arity operators.
					//we have prefixes and postfixes and binaries, so sin, !, ^2 etc, but no - as a negative. therefore only one operation between operands
					
					//Important! there is no reason for postfix operators to accumulate, as they should happen before other postfixes. and after 
					//prefixes
					else
					{
						if (operation is UnaryOperationToken unary && unary.Fixity == Fixity.Postfix)
						{
							//Operation is postfix eg ! so to deal with this, we will continue peeking and evaluating until we have an operation with lower precedence
							//the goal is to get increasing chain +, *, ^ so we can retain preference when executing in reverse :)

							//I currently dont have a way of *enforcing* this but if you put all the dual arities as dual arities when they are binary
							//and put their unary properties when they are unary, you shouldnt have to deal with duals that are ambiguous
							//ex in the below, they would be classified as binaries, fittingly.

							//context: we have encountered a postfix, and we need to remove all the operations with higher priority before this.
							//We know the postfix is not the only operator, or it would have been processed already. therefore there is at least one operator.
							//keep going until there are no more operators, or the priority of the operator before is lower
							var lastOperationBeforePostfix = opStack.Peek();
							double result;
							while (opStack.Count > 0 && opStack.Peek().Priority > unary.Priority)
							{
								var firstOperand = numStack.Pop();
								lastOperationBeforePostfix = opStack.Pop();

								if (lastOperationBeforePostfix is IBinaryOperation lastOperationBinary)
								{
									var secondOperand = numStack.Pop();
									result = lastOperationBinary.CalculateBinary(firstOperand.Calculate(), secondOperand.Calculate());
								}
								else
								{
									var lastOperationUnary = lastOperationBeforePostfix as IUnaryOperation;
									result = lastOperationUnary!.CalculateUnary(firstOperand.Calculate());
								}

								numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
								if (opStack.Count == 0)
								{
									break;
								}
							}

							var operand = numStack.Pop();
							result = unary.CalculateUnary(operand.Calculate());
							numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
						}

						else if (operation is IBinaryOperation binaryOperation && binaryOperation.Associativity == Associativity.Left)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority >= binaryOperation.Priority )
							{
								var op = opStack.Pop();
								var latestOperand = numStack.Pop();
								double result;
								if (op is IBinaryOperation opBinary)
								{
									var earlierOperand = numStack.Pop();
									result = opBinary!.CalculateBinary(earlierOperand.Calculate(), latestOperand.Calculate());
								}
								else
								{
									var unaryOperation = op as IUnaryOperation;
									result = unaryOperation!.CalculateUnary(latestOperand.Calculate());
								}

								numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
							}

							opStack.Push(binaryOperation);
						}
						else if (operation is IBinaryOperation binaryOperationRight)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority > binaryOperationRight.Priority)
							{
								var op = opStack.Pop();
								var laterOperand = numStack.Pop();
								double result;
								if (op is IBinaryOperation binaryOperator)
								{
									var earlierOperand = numStack.Pop();
									result = binaryOperator!.CalculateBinary(earlierOperand.Calculate(), laterOperand.Calculate());

								}
								else
								{
									var unaryOperation = op as IUnaryOperation;
									result = unaryOperation!.CalculateUnary(laterOperand.Calculate());
								}

								numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
							}

							opStack.Push(binaryOperationRight);
						}
					}
				}

				currentIndex++;
			}

			//I think this works, but i am not sure
			while (opStack.Count > 0)
			{
				var trailingOperation = opStack.Pop();
				var latestOperand = numStack.Pop();
				double result;

				if (trailingOperation is IBinaryOperation binaryOperation)
				{
					var earlierOperand = numStack.Pop();
					result = binaryOperation.CalculateBinary(earlierOperand.Calculate(), latestOperand.Calculate());
				}
				else
				{
					//make sure this wont be null, as every operation should either be ibinary or iunary
					var unaryOperation = trailingOperation as IUnaryOperation;
					result = unaryOperation!.CalculateUnary(latestOperand.Calculate());
				}

				numStack.Push(NullaryOperationToken.GetConstFromDouble(result));
			}

			return numStack.Pop();
		}

		//Write test cases
		public void InsertMultiplicationSignsConvertUnaryDualsToUnaryPlaceBrackets()
		{
			IToken currentToken, nextToken;
			var unmatchedOpenBraces = 0;
			for (int i = 0; i < operations.Count - 1; i++)
			{
				currentToken = operations[i];
				nextToken = operations[i + 1];

				if ((currentToken == OperationDefinitions.ClosedBracket || currentToken is NullaryOperationToken or DigitToken || currentToken is UnaryOperationToken thisUnary && thisUnary.Fixity == Fixity.Postfix) &&
					(nextToken == OperationDefinitions.OpenBracket || nextToken is NullaryOperationToken || nextToken is UnaryOperationToken nextUnary && nextUnary.Fixity == Fixity.Prefix))
				{
					InsertAt(i + 1, OperationDefinitions.Multiplication);
					i++;
				}
			}

			var isInOperationString = true;

			for (int i = 0; i < operations.Count; i++)
			{
				if (operations[i] == OperationDefinitions.OpenBracket)
				{
					unmatchedOpenBraces++;
				}
				else if (operations[i] == OperationDefinitions.ClosedBracket)
				{
					unmatchedOpenBraces--;
				}
				//Logic: we can have an operation string. we enter a string by placing a dual, binary, or unary
				//once one is entered, we set bool is in string to true, and when it is true, we only place unaries
				if (operations[i] is DualArityOperationToken dual)
				{
					if (isInOperationString)
					{
						operations[i] = dual.UnaryOperation;
					}
					else
					{
						isInOperationString = true;
					}
				}
				else if (operations[i] is IOperation || operations[i] == OperationDefinitions.OpenBracket)
				{
					isInOperationString = true;
				}
				else
				{
					isInOperationString = false;
				}
			}

			for (int i = 0; i < unmatchedOpenBraces; i++)
			{
				Add(OperationDefinitions.ClosedBracket);
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
				index++;
			}

			var result = double.Parse(resultAsString);
			return NullaryOperationToken.GetConstFromDouble(result);
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
				if (operations[i] == OperationDefinitions.OpenBracket)
				{
					bracketStack.Push(i);
				}
				else if (operations[i] == OperationDefinitions.ClosedBracket)
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
			return !(finalToken == OperationDefinitions.OpenBracket || finalToken is UnaryOperationToken finalUnary && finalUnary.Fixity == Fixity.Prefix || finalToken is BinaryOperationToken || finalToken is DualArityOperationToken dual && dual.UnaryOperation.Fixity == Fixity.Prefix);
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
