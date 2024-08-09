using MultiCalculator.Abstractions;
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
			ButtonA0.ButtonOperation = new UnaryButtonOperation() { DisplayName = "Abs", Calculate = Math.Abs };
			ButtonA1.ButtonOperation = new UnaryButtonOperation() { DisplayName = "x³", Calculate = (x) => x * x * x };
			ButtonA4.ButtonOperation = new UnaryButtonOperation() { DisplayName = "x⁻¹", Calculate = (x) => 1 / x };
			ButtonA5.ButtonOperation = new UnaryButtonOperation() { DisplayName = "x!", Calculate = MathHelpers.Factorial };

			ButtonB0.ButtonOperation = new UnaryButtonOperation() { DisplayName = "√", Calculate = Math.Sqrt };
			ButtonB0.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "∛", Calculate = Math.Cbrt };
			ButtonB1.ButtonOperation = new UnaryButtonOperation() { DisplayName = "x²", Calculate = (x) => x * x };
			ButtonB2.ButtonOperation = new BracketButtonOperation() { DisplayName = "(", BracketType = BracketType.Open };
			ButtonB3.ButtonOperation = new BracketButtonOperation() { DisplayName = ")", BracketType = BracketType.Closed };
			ButtonB4.ButtonOperation = new UnaryButtonOperation() { DisplayName = "log", Calculate = Math.Log10 };
			ButtonB4.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "10▫", Calculate = (x) => Math.Pow(10, x) };
			ButtonB5.ButtonOperation = new UnaryButtonOperation() { DisplayName = "ln", Calculate = Math.Log };
			ButtonB5.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "e▫", Calculate = (x) => Math.Pow(10, x) };

			ButtonC0.ButtonOperation = new BinaryButtonOperation() { DisplayName = "x▫", Calculate = Math.Pow };
			ButtonC0.SecondaryButtonOperation = new BinaryButtonOperation() { DisplayName = "▫√", Calculate = (x, y) => Math.Pow(x, 1 / y) };
			ButtonC1.ButtonOperation = new NullaryButtonOperation() { DisplayName = "e", Calculate = () => Math.E };
			ButtonC1.SecondaryButtonOperation = new NullaryButtonOperation() { DisplayName = "γ", Calculate = () => 0.5772156649015329 };
			ButtonC2.ButtonOperation = new NullaryButtonOperation() { DisplayName = "π", Calculate = () => Math.PI };
			ButtonC2.SecondaryButtonOperation = new NullaryButtonOperation() { DisplayName = "φ", Calculate = () => (1 + Math.Sqrt(5)) / 2 };
			ButtonC3.ButtonOperation = new UnaryButtonOperation() { DisplayName = "sin", Calculate = Math.Sin };
			ButtonC3.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "asin", Calculate = Math.Asin };
			ButtonC4.ButtonOperation = new UnaryButtonOperation() { DisplayName = "cos", Calculate = Math.Cos };
			ButtonC4.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "acos", Calculate = Math.Acos };
			ButtonC5.ButtonOperation = new UnaryButtonOperation() { DisplayName = "tan", Calculate = Math.Tan };
			ButtonC5.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "atan", Calculate = Math.Tan };

			ButtonD0.ButtonOperation = new BinaryButtonOperation() { DisplayName = "nPr", Calculate = MathHelpers.P };
			ButtonD0.SecondaryButtonOperation = new BinaryButtonOperation() { DisplayName = "nCr", Calculate = MathHelpers.C };
			ButtonD1.ButtonOperation = new UnaryButtonOperation() { DisplayName = "erf(x)", Calculate = MathHelpers.Erf };
			ButtonD1.SecondaryButtonOperation = new NullaryButtonOperation() { DisplayName = "ϖ", Calculate = () => 2.6220575542921198 };
			ButtonD2.ButtonOperation = new UnaryButtonOperation() { DisplayName = "W(x)", Calculate = MathHelpers.LambertW };
			ButtonD2.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "sinc", Calculate = (x) => Math.Sin(x) / x };
			ButtonD3.ButtonOperation = new UnaryButtonOperation() { DisplayName = "sinh", Calculate = Math.Sinh };
			ButtonD3.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "asinh", Calculate = Math.Asinh };
			ButtonD4.ButtonOperation = new UnaryButtonOperation() { DisplayName = "cosh", Calculate = Math.Cosh };
			ButtonD4.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "acosh", Calculate = Math.Acosh };
			ButtonD5.ButtonOperation = new UnaryButtonOperation() { DisplayName = "tanh", Calculate = Math.Tanh };
			ButtonD5.SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "atanh", Calculate = Math.Atanh };
		}

		public void DoCalculator(object sender, RoutedEventArgs e)
		{
			//This needs to be changed to work for shiftablebutton OR button
			var x = sender as ICalculatorButton;
			if (x is null)
			{
				throw new MultiCalculatorException();
			}
			var ubtn = (UnaryButtonOperation)x.ButtonOperation;
			if (ubtn is null)
			{
				throw new MultiCalculatorException();
			}
			var result = ubtn.Calculate(3);
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
