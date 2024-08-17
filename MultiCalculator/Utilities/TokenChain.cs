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

		public static TokenChain Duplicate(TokenChain initial)
		{
			var result = new TokenChain();
			foreach (var operation in initial.operations)
			{
				result.operations.Add(operation);
			}
			return result;
		}

		public override string ToString()
		{
			var hasCursorBeenPlaced = false;
			var result = string.Empty;
			for (int i = 0; i < operations.Count; i++)
			{
				if (i == Cursor)
				{
					result += "|";
					hasCursorBeenPlaced = true;
				}
				result += operations[i].TokenSymbol;
			}
			if (!hasCursorBeenPlaced)
			{
				result += "|";
			}
			return result;
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
			return HasNonEmptyAndNonNegativeOpenBraces() && NumbersExistAndAreWellFormed() && NoConsecutiveBinaryOperations() && NoDigitsFollowClosingBrace() && ExpressionDoesNotEndInOperation() && NoIllegalConsecutiveTokens();
		}

		//This is only called on valid strings
		public string GetLatexString()
		{
			var chainCopy = Duplicate(this);
			chainCopy.InsertMultiplicationSignsConvertUnaryDualsToUnaryPlaceBrackets();

			var tree = chainCopy.ParseFromIndexToIndexKeepInTreeForm(0, chainCopy.operations.Count);
			var latexString = tree.GetLatexString();
			return latexString;
		}

		public double ParseTree()
		{
			return ParseFromIndexToIndexProductConst(0, operations.Count).Evaluate().Calculate();
		}

		TreeNode<IToken> ParseFromIndexToIndexProductConst(int startIndex, int endIndexExclusive)
		{
			var numStackIncludingTrees = new Stack<TreeNode<IToken>>();
			var currentIndex = startIndex;
			var opStack = new Stack<IOperation>();

			while (currentIndex < endIndexExclusive)
			{
				var currentToken = operations[currentIndex];

				if (currentToken is NullaryOperationToken constant)
				{
					numStackIncludingTrees.Push(new TreeNode<IToken>(constant));
				}

				else if (currentToken is DigitToken)
				{
					var parsedNumberAsConst = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStackIncludingTrees.Push(new TreeNode<IToken>(parsedNumberAsConst));
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

					numStackIncludingTrees.Push(ParseFromIndexToIndexProductConst(currentIndex + 1, currentIndex + nextClosingBraceDistance - 1));
					currentIndex += nextClosingBraceDistance - 1;
				}

				else if (currentToken is UnaryOperationToken unaryPrefix && unaryPrefix.Fixity == Fixity.Prefix)
				{
					opStack.Push(unaryPrefix);
				}

				else if (currentToken is IOperation operation)
				{
					if (opStack.Count == 0 && !(operation is UnaryOperationToken firstOperation && firstOperation.Fixity == Fixity.Postfix))
					{
						opStack.Push(operation);
					}

					else if (opStack.Count == 0)
					{
						var unaryOperation = operation as UnaryOperationToken;
						var unaryOperand = numStackIncludingTrees.Pop().Evaluate();
						var result = unaryOperation!.CalculateUnary(unaryOperand.Calculate());
						numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
					}

					else
					{
						if (operation is UnaryOperationToken unary && unary.Fixity == Fixity.Postfix)
						{
							var lastOperationBeforePostfix = opStack.Peek();
							double result;
							while (opStack.Count > 0 && opStack.Peek().Priority > unary.Priority)
							{
								//Mukund: these operands are the wrong way!
								var firstOperand = numStackIncludingTrees.Pop().Evaluate();
								lastOperationBeforePostfix = opStack.Pop();

								if (lastOperationBeforePostfix is IBinaryOperation lastOperationBinary)
								{
									var secondOperand = numStackIncludingTrees.Pop().Evaluate();
									result = lastOperationBinary.CalculateBinary(secondOperand.Calculate(), firstOperand.Calculate());
								}
								else
								{
									var lastOperationUnary = lastOperationBeforePostfix as IUnaryOperation;
									result = lastOperationUnary!.CalculateUnary(firstOperand.Calculate());
								}

								numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
								if (opStack.Count == 0)
								{
									break;
								}
							}

							var operand = numStackIncludingTrees.Pop().Evaluate();
							result = unary.CalculateUnary(operand.Calculate());
							numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
						}

						else if (operation is IBinaryOperation binaryOperation && binaryOperation.Associativity == Associativity.Left)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority >= binaryOperation.Priority)
							{
								var op = opStack.Pop();
								var latestOperand = numStackIncludingTrees.Pop().Evaluate();
								double result;
								if (op is IBinaryOperation opBinary)
								{
									var earlierOperand = numStackIncludingTrees.Pop().Evaluate();
									result = opBinary!.CalculateBinary(earlierOperand.Calculate(), latestOperand.Calculate());
								}
								else
								{
									var unaryOperation = op as IUnaryOperation;
									result = unaryOperation!.CalculateUnary(latestOperand.Calculate());
								}

								numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
							}

							opStack.Push(binaryOperation);
						}
						else if (operation is IBinaryOperation binaryOperationRight)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority > binaryOperationRight.Priority)
							{
								var op = opStack.Pop();
								var laterOperand = numStackIncludingTrees.Pop().Evaluate();
								double result;
								if (op is IBinaryOperation binaryOperator)
								{
									var earlierOperand = numStackIncludingTrees.Pop().Evaluate();
									result = binaryOperator!.CalculateBinary(earlierOperand.Calculate(), laterOperand.Calculate());

								}
								else
								{
									var unaryOperation = op as IUnaryOperation;
									result = unaryOperation!.CalculateUnary(laterOperand.Calculate());
								}

								numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
							}

							opStack.Push(binaryOperationRight);
						}
					}
				}

				currentIndex++;
			}

			while (opStack.Count > 0)
			{
				var trailingOperation = opStack.Pop();
				var latestOperand = numStackIncludingTrees.Pop().Evaluate();
				double result;

				var trailingTreeNode = new TreeNode<IToken>(trailingOperation);
				trailingTreeNode.AddChild(latestOperand);

				if (trailingOperation is IBinaryOperation binaryOperation)
				{
					var earlierOperand = numStackIncludingTrees.Pop().Evaluate();
					result = binaryOperation.CalculateBinary(earlierOperand.Calculate(), latestOperand.Calculate());
					trailingTreeNode.InsertChildAt(earlierOperand, 0);
				}
				else
				{
					var unaryOperation = trailingOperation as IUnaryOperation;
					result = unaryOperation!.CalculateUnary(latestOperand.Calculate());
				}

				numStackIncludingTrees.Push(new TreeNode<IToken>(NullaryOperationToken.GetConstFromDouble(result)));
			}

			return numStackIncludingTrees.Pop();
		}

		TreeNode<IToken> ParseFromIndexToIndexKeepInTreeForm(int startIndex, int endIndexExclusive)
		{
			var numStackIncludingTrees = new Stack<TreeNode<IToken>>();
			var currentIndex = startIndex;
			var opStack = new Stack<IOperation>();

			while (currentIndex < endIndexExclusive)
			{
				var currentToken = operations[currentIndex];

				if (currentToken is NullaryOperationToken constant)
				{
					numStackIncludingTrees.Push(new TreeNode<IToken>(constant));
				}

				else if (currentToken is DigitToken)
				{
					var parsedNumberAsConst = ParseDoubleFromIndex(currentIndex, out int lengthOfNumber);
					numStackIncludingTrees.Push(new TreeNode<IToken>(parsedNumberAsConst));
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

					var nestedTreeResult = ParseFromIndexToIndexKeepInTreeForm(currentIndex + 1, currentIndex + nextClosingBraceDistance - 1);
					var bracketWrapper = new TreeNode<IToken>(OperationDefinitions.Braces);
					bracketWrapper.AddTree(nestedTreeResult);
					numStackIncludingTrees.Push(bracketWrapper);
					currentIndex += nextClosingBraceDistance - 1;
				}

				else if (currentToken is UnaryOperationToken unaryPrefix && unaryPrefix.Fixity == Fixity.Prefix)
				{
					opStack.Push(unaryPrefix);
				}

				else if (currentToken is IOperation operation)
				{
					if (opStack.Count == 0 && !(operation is UnaryOperationToken firstOperation && firstOperation.Fixity == Fixity.Postfix))
					{
						opStack.Push(operation);
					}

					else if (opStack.Count == 0)
					{
						var unaryOperation = operation as UnaryOperationToken;
						var unaryOperand = numStackIncludingTrees.Pop();
						var resultTree = new TreeNode<IToken>(unaryOperation!);
						resultTree.AddTree(unaryOperand);
						numStackIncludingTrees.Push(resultTree);
					}

					else
					{
						if (operation is UnaryOperationToken unary && unary.Fixity == Fixity.Postfix)
						{
							var unaryPostfixTree = new TreeNode<IToken>(unary);
							while (opStack.Count > 0 && opStack.Peek().Priority > unary.Priority)
							{
								var laterOperand = numStackIncludingTrees.Pop();
								var lastOperationBeforePostfix = opStack.Pop();

								var resultTree = new TreeNode<IToken>(lastOperationBeforePostfix);
								resultTree.AddTree(laterOperand);

								if (lastOperationBeforePostfix is IBinaryOperation lastOperationBinary)
								{
									var earlierOperand = numStackIncludingTrees.Pop();
									resultTree.InsertTreeAt(earlierOperand, 0);
								}

								numStackIncludingTrees.Push(resultTree);
								if (opStack.Count == 0)
								{
									break;
								}
							}

							var operand = numStackIncludingTrees.Pop();
							unaryPostfixTree.AddTree(operand);

							numStackIncludingTrees.Push(unaryPostfixTree);
						}

						else if (operation is IBinaryOperation binaryOperation && binaryOperation.Associativity == Associativity.Left)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority >= binaryOperation.Priority)
							{
								var op = opStack.Pop();
								var latestOperand = numStackIncludingTrees.Pop();
								var resultTree = new TreeNode<IToken>(op);
								resultTree.AddTree(latestOperand);

								if (op is IBinaryOperation)
								{
									var earlierOperand = numStackIncludingTrees.Pop();
									resultTree.InsertTreeAt(earlierOperand, 0);
								}

								numStackIncludingTrees.Push(resultTree);
							}

							opStack.Push(binaryOperation);
						}
						else if (operation is IBinaryOperation binaryOperationRight)
						{
							while (opStack.Count > 0 && opStack.Peek().Priority > binaryOperationRight.Priority)
							{
								var op = opStack.Pop();
								var resultTree = new TreeNode<IToken>(op);
								var laterOperand = numStackIncludingTrees.Pop();
								resultTree.AddTree(laterOperand);

								if (op is IBinaryOperation)
								{
									var earlierOperand = numStackIncludingTrees.Pop();
									resultTree.InsertTreeAt(earlierOperand, 0);
								}

								numStackIncludingTrees.Push(resultTree);
							}

							opStack.Push(binaryOperationRight);
						}
					}
				}

				currentIndex++;
			}

			while (opStack.Count > 0)
			{
				var trailingOperation = opStack.Pop();
				var latestOperand = numStackIncludingTrees.Pop();

				var trailingTreeNode = new TreeNode<IToken>(trailingOperation);
				trailingTreeNode.AddTree(latestOperand);

				if (trailingOperation is IBinaryOperation)
				{
					var earlierOperand = numStackIncludingTrees.Pop();
					trailingTreeNode.InsertTreeAt(earlierOperand, 0);
				}

				numStackIncludingTrees.Push(trailingTreeNode);
			}

			return numStackIncludingTrees.Pop();
		}

		public void InsertMultiplicationSignsConvertUnaryDualsToUnaryPlaceBrackets()
		{
			InsertMultiplicationSigns();
			ConvertDualUnariesToUnariesAndPlaceClosingBrackets();
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

		public void InsertMultiplicationSigns()
		{
			IToken currentToken, nextToken;
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
		}

		public void ConvertDualUnariesToUnariesAndPlaceClosingBrackets()
		{
			var unmatchedOpenBraces = 0;
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
				InsertAt(operations.Count, OperationDefinitions.ClosedBracket);
			}
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

		bool NoIllegalConsecutiveTokens()
		{
			IToken currentToken, nextToken;
			for (int i = 0; i < operations.Count - 1; i++)
			{
				currentToken = operations[i];
				nextToken = operations[i + 1];

				if ((currentToken == OperationDefinitions.OpenBracket || currentToken is BinaryOperationToken or DualArityOperationToken || currentToken is UnaryOperationToken unary && unary.Fixity == Fixity.Prefix)
					&& (nextToken == OperationDefinitions.ClosedBracket || nextToken  is BinaryOperationToken || nextToken is UnaryOperationToken unaryPost && unaryPost.Fixity == Fixity.Postfix))
				{
					return false;
				}

				if ((currentToken == OperationDefinitions.ClosedBracket || currentToken is NullaryOperationToken || currentToken is UnaryOperationToken unaryPostFix && unaryPostFix.Fixity == Fixity.Postfix) && nextToken is DigitToken)
				{
					return false;
				}
			}

			return true;
		}

		bool NoConsecutiveBinaryOperations()
		{
			bool currentOperationIsBinary = true;

			foreach (var operation in operations)
			{
				if ((operation is BinaryOperationToken || operation is UnaryOperationToken unary && unary.Fixity == Fixity.Postfix) && currentOperationIsBinary)
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
