using MultiCalculator.Abstractions;
using MultiCalculator.Enums;
using MultiCalculator.Exceptions;
using MultiCalculator.Helpers;
using MultiCalculator.Implementations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for ScientificFunctions.xaml
	/// </summary>
	public partial class ScientificFunctions : UserControl
	{
		public ScientificFunctions()
		{
			InitializeComponent();

			//Note: these symbols may not render. In the long term, move to images on buttons.
			ButtonA0.ButtonOperation = new UnaryOperationToken() { DisplayName = "Abs", CalculateUnary = Math.Abs };
			ButtonA1.ButtonOperation = new UnaryOperationToken() { DisplayName = "x³", CalculateUnary = (x) => x * x * x };
			ButtonA4.ButtonOperation = new UnaryOperationToken() { DisplayName = "x⁻¹", CalculateUnary = (x) => 1 / x };
			ButtonA5.ButtonOperation = new UnaryOperationToken() { DisplayName = "x!", CalculateUnary = MathHelpers.Factorial };

			ButtonB0.ButtonOperation = new UnaryOperationToken() { DisplayName = "√", CalculateUnary = Math.Sqrt };
			ButtonB0.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "∛", CalculateUnary = Math.Cbrt };
			ButtonB1.ButtonOperation = new UnaryOperationToken() { DisplayName = "x²", CalculateUnary = (x) => x * x };
			ButtonB2.ButtonOperation = new BracketToken() { DisplayName = "(", BracketType = BracketType.Open };
			ButtonB3.ButtonOperation = new BracketToken() { DisplayName = ")", BracketType = BracketType.Closed };
			ButtonB4.ButtonOperation = new UnaryOperationToken() { DisplayName = "log", CalculateUnary = Math.Log10 };
			ButtonB4.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "10▫", CalculateUnary = (x) => Math.Pow(10, x) };
			ButtonB5.ButtonOperation = new UnaryOperationToken() { DisplayName = "ln", CalculateUnary = Math.Log };
			ButtonB5.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "e▫", CalculateUnary = (x) => Math.Pow(10, x) };

			ButtonC0.ButtonOperation = new BinaryOperationToken() { DisplayName = "x▫", CalculateBinary = Math.Pow };
			ButtonC0.SecondaryButtonOperation = new BinaryOperationToken() { DisplayName = "▫√", CalculateBinary = (x, y) => Math.Pow(x, 1 / y) };
			ButtonC1.ButtonOperation = new NullaryOperationToken() { DisplayName = "e", Calculate = () => Math.E };
			ButtonC1.SecondaryButtonOperation = new NullaryOperationToken() { DisplayName = "γ", Calculate = () => 0.5772156649015329 };
			ButtonC2.ButtonOperation = new NullaryOperationToken() { DisplayName = "π", Calculate = () => Math.PI };
			ButtonC2.SecondaryButtonOperation = new NullaryOperationToken() { DisplayName = "φ", Calculate = () => (1 + Math.Sqrt(5)) / 2 };
			ButtonC3.ButtonOperation = new UnaryOperationToken() { DisplayName = "sin", CalculateUnary = Math.Sin };
			ButtonC3.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "asin", CalculateUnary = Math.Asin };
			ButtonC4.ButtonOperation = new UnaryOperationToken() { DisplayName = "cos", CalculateUnary = Math.Cos };
			ButtonC4.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "acos", CalculateUnary = Math.Acos };
			ButtonC5.ButtonOperation = new UnaryOperationToken() { DisplayName = "tan", CalculateUnary = Math.Tan };
			ButtonC5.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "atan", CalculateUnary = Math.Tan };

			ButtonD0.ButtonOperation = new BinaryOperationToken() { DisplayName = "nPr", CalculateBinary = MathHelpers.P };
			ButtonD0.SecondaryButtonOperation = new BinaryOperationToken() { DisplayName = "nCr", CalculateBinary = MathHelpers.C };
			ButtonD1.ButtonOperation = new UnaryOperationToken() { DisplayName = "erf", CalculateUnary = MathHelpers.Erf };
			ButtonD1.SecondaryButtonOperation = new NullaryOperationToken() { DisplayName = "ϖ", Calculate = () => 2.6220575542921198 };
			ButtonD2.ButtonOperation = new UnaryOperationToken() { DisplayName = "W(x)", CalculateUnary = MathHelpers.LambertW };
			ButtonD2.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "sinc", CalculateUnary = (x) => Math.Sin(x) / x };
			ButtonD3.ButtonOperation = new UnaryOperationToken() { DisplayName = "sinh", CalculateUnary = Math.Sinh };
			ButtonD3.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "asinh", CalculateUnary = Math.Asinh };
			ButtonD4.ButtonOperation = new UnaryOperationToken() { DisplayName = "cosh", CalculateUnary = Math.Cosh };
			ButtonD4.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "acosh", CalculateUnary = Math.Acosh };
			ButtonD5.ButtonOperation = new UnaryOperationToken() { DisplayName = "tanh", CalculateUnary = Math.Tanh };
			ButtonD5.SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "atanh", CalculateUnary = Math.Atanh };
		}

		public void ScientificButton_Click(object sender, RoutedEventArgs e)
		{
			//This needs to be changed to work for shiftablebutton OR button
			var x = sender as ICalculatorButton;
			if (x is null)
			{
				throw new MultiCalculatorException();
			}
			var ubtn = (UnaryOperationToken)x.ButtonOperation;
			if (ubtn is null)
			{
				throw new MultiCalculatorException();
			}
			var result = ubtn.CalculateUnary(3);
		}

		public void ToggleAllButtons()
		{
			ToggleAllButtons(this);
		}

		static void ToggleAllButtons(DependencyObject parent)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (child is ShiftableButton shiftableButton)
				{
					shiftableButton.Toggle();
				}
				else
				{
					ToggleAllButtons(child);
				}
			}
		}
	}
}
