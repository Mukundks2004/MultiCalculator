using System.Windows;
using System.Windows.Controls;

using static MultiCalculator.Definitions.OperationDefinitions;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for ScientificFunctions.xaml
	/// </summary>
	public partial class ScientificFunctions : UserControl
	{
		public event EventHandler? WriteTokenToScreen;

		public ScientificFunctions()
		{
			InitializeComponent();

			//Note: these symbols may not render. In the long term, move to images on buttons.
			ButtonA0.CalculatorTask.Name = "Abs";
			ButtonA0.CalculatorTask.Add(Abs);
			ButtonA1.CalculatorTask.Name = "x³";
			ButtonA1.CalculatorTask.Add(Exponentiation);
			ButtonA1.CalculatorTask.Add(Three);
			ButtonA4.CalculatorTask.Name = "x⁻¹";
			ButtonA4.CalculatorTask.Add(Exponentiation);
			ButtonA4.CalculatorTask.Add(Subtraction);
			ButtonA4.CalculatorTask.Add(One);
			ButtonA5.CalculatorTask.Name = "x!";
			ButtonA5.CalculatorTask.Add(Factorial);

			ButtonB0.CalculatorTask.Name = "√";
			ButtonB0.CalculatorTask.Add(Sqrt);
			ButtonB0.SecondaryCalculatorTask.Name = "∛";
			ButtonB0.SecondaryCalculatorTask.Add(Cbrt);
			ButtonB1.CalculatorTask.Name = "x²";
			ButtonB1.CalculatorTask.Add(Exponentiation);
			ButtonB1.CalculatorTask.Add(Two);
			ButtonB2.CalculatorTask.Name = "(";
			ButtonB2.CalculatorTask.Add(OpenBracket);
			ButtonB3.CalculatorTask.Name = ")";
			ButtonB3.CalculatorTask.Add(ClosedBracket);
			ButtonB4.CalculatorTask.Name = "log";
			ButtonB4.CalculatorTask.Add(Log);
			ButtonB4.SecondaryCalculatorTask.Name = "10▫";
			ButtonB4.SecondaryCalculatorTask.Add(Antilog);
			ButtonB5.CalculatorTask.Name = "ln";
			ButtonB5.CalculatorTask.Add(Ln);
			ButtonB5.SecondaryCalculatorTask.Name = "e▫";
			ButtonB5.SecondaryCalculatorTask.Add(Exp);

			ButtonC0.CalculatorTask.Name = "x▫";
			ButtonC0.CalculatorTask.Add(Exponentiation);
			ButtonC0.SecondaryCalculatorTask.Name = "▫√";
			ButtonC0.SecondaryCalculatorTask.Add(Nthroot);
			ButtonC1.CalculatorTask.Name = "e";
			ButtonC1.CalculatorTask.Add(E);
			ButtonC1.SecondaryCalculatorTask.Name = "γ";
			ButtonC1.SecondaryCalculatorTask.Add(Mascheroni);
			ButtonC2.CalculatorTask.Name = "π";
			ButtonC2.CalculatorTask.Add(Pi);
			ButtonC2.SecondaryCalculatorTask.Name = "φ";
			ButtonC2.SecondaryCalculatorTask.Add(Phi);
			ButtonC3.CalculatorTask.Name = "sin";
			ButtonC3.CalculatorTask.Add(Sin);
			ButtonC3.SecondaryCalculatorTask.Name = "asin";
			ButtonC3.SecondaryCalculatorTask.Add(Asin);
			ButtonC4.CalculatorTask.Name = "cos";
			ButtonC4.CalculatorTask.Add(Cos);
			ButtonC4.SecondaryCalculatorTask.Name = "acos";
			ButtonC4.SecondaryCalculatorTask.Add(Acos);
			ButtonC5.CalculatorTask.Name = "tan";
			ButtonC5.CalculatorTask.Add(Tan);
			ButtonC5.SecondaryCalculatorTask.Name = "atan";
			ButtonC5.SecondaryCalculatorTask.Add(Atan);

			ButtonD0.CalculatorTask.Name = "nPr";
			ButtonD0.CalculatorTask.Add(Permuations);
			ButtonD0.SecondaryCalculatorTask.Name = "nCr";
			ButtonD0.SecondaryCalculatorTask.Add(Combinations);
			ButtonD1.CalculatorTask.Name = "erf";
			ButtonD1.CalculatorTask.Add(Erf);
			ButtonD1.SecondaryCalculatorTask.Name = "ϖ";
			ButtonD1.SecondaryCalculatorTask.Add(Lemniscate);
			ButtonD2.CalculatorTask.Name = "W(x)";
			ButtonD2.CalculatorTask.Add(Productlog);
			ButtonD2.SecondaryCalculatorTask.Name = "sinc";
			ButtonD2.SecondaryCalculatorTask.Add(Sinc);
			ButtonD3.CalculatorTask.Name = "sinh";
			ButtonD3.CalculatorTask.Add(Sinh);
			ButtonD3.SecondaryCalculatorTask.Name = "asinh";
			ButtonD3.SecondaryCalculatorTask.Add(Asinh);
			ButtonD4.CalculatorTask.Name = "cosh";
			ButtonD4.CalculatorTask.Add(Cosh);
			ButtonD4.SecondaryCalculatorTask.Name = "acosh";
			ButtonD4.SecondaryCalculatorTask.Add(Acosh);
			ButtonD5.CalculatorTask.Name = "tanh";
			ButtonD5.CalculatorTask.Add(Tanh);
			ButtonD5.SecondaryCalculatorTask.Name = "atanh";
			ButtonD5.SecondaryCalculatorTask.Add(Atanh);
		}

		public void ScientificButton_Click(object sender, RoutedEventArgs e)
		{
			WriteTokenToScreen?.Invoke(sender, EventArgs.Empty);
		}
	}
}
