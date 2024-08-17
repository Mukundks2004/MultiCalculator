using MultiCalculator.Abstractions;
using MultiCalculator.Exceptions;
using MultiCalculator.Implementations;
using System.Web;

namespace MultiCalculator.Utilities
{
	public class TreeNode<T> where T : IToken
	{
		public T Value { get; set; }

		public List<TreeNode<T>> Children { get; private set; }

		public TreeNode(T value)
		{
			Value = value;
			Children = [];
		}

		public void AddChild(T value)
		{
			Children.Add(new TreeNode<T>(value));
		}

		public void AddTree(TreeNode<T> childTree)
		{
			Children.Add(childTree);
		}

		public void InsertChildAt(T value, int index)
		{
			Children.Insert(index, new TreeNode<T>(value));
		}

		public void InsertTreeAt(TreeNode<T> childTree, int index)
		{
			Children.Insert(index, childTree);
		}

		public TreeNode<T> GetChild(int index)
		{
			return Children[index];
		}

		public void PrintTree(string indent = "")
		{
			Console.WriteLine(indent + Value);
			foreach (var child in Children)
			{
				child.PrintTree(indent + "  ");
			}
		}

		public NullaryOperationToken Evaluate()
		{
			if (Value is NullaryOperationToken nullary)
			{
				return nullary;
			}
			else if (Value is UnaryOperationToken unary)
			{
				return new NullaryOperationToken() { Calculate = () => unary.CalculateUnary(Children[0].Evaluate().Calculate()) };
			}
			else if (Value is BinaryOperationToken binary)
			{
				return new NullaryOperationToken()
				{
					Calculate = () => binary.CalculateBinary(Children[0].Evaluate().Calculate(), Children[1].Evaluate().Calculate())
				};
			}
			else if (Value is DualArityOperationToken dual)
			{
				return new NullaryOperationToken()
				{
					Calculate = () => dual.CalculateBinary(Children[0].Evaluate().Calculate(), Children[1].Evaluate().Calculate())
				};
			}
			else
			{
				throw new MultiCalculatorException("How did this value get inside the AST?");
			}
        }

		//This is only called on valid strings
		public string GetLatexString()
		{
			var result = string.Empty;

			if (Value is NullaryOperationToken constant)
			{
				result += constant.LatexString;
			}
			else if (Value is DigitToken digit)
			{
				//impossible case
				result += digit.LatexString;
			}
			else if (Value is BracketToken bracket)
			{
				result += bracket.LatexString;
			}
			else if (Value is UnaryOperationToken unary)
			{
				result += unary.LatexString(Children[0].GetLatexString());
			}
			else if (Value is BinaryOperationToken binary)
			{
				result += binary.LatexString(Children[0].GetLatexString(), Children[1].GetLatexString());
			}
			else if (Value is DualArityOperationToken dual)
			{
				result += dual.LatexString(Children[0].GetLatexString(), Children[1].GetLatexString());
			}
            else
            {
				throw new MultiCalculatorException("Unknown type in AST");
            }

            return result;
		}
	}
}
