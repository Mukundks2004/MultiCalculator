using ceTe.DynamicPDF.PageElements.BarCoding;
using MultiCalculator.Delegates;
using MultiCalculator.Implementations;
using System.Windows;
using System.Windows.Controls;

using static MultiCalculator.Definitions.OperationDefinitions;

namespace MultiCalculator.Controls
{
    /// <summary>
    /// Interaction logic for PrimaryFunctions.xaml
    /// </summary>
    public partial class PrimaryFunctions : UserControl
	{
		public event EventHandler? WriteTokenToScreen;

		public event SimpleEventHandler? EvaluateExpression;

		public event SimpleEventHandler? ToggleShiftables;

		public event SimpleEventHandler? BackspaceToken;

		public event SimpleEventHandler? ClearEntireExpression;

		public event SimpleEventHandler? GetLastAnswer;

		public PrimaryFunctions()
		{
			InitializeComponent();

			ButtonX0.CalculatorTask.Name = "7";
			ButtonX0.CalculatorTask.Add(Seven);
			ButtonX1.CalculatorTask.Name = "8";
			ButtonX1.CalculatorTask.Add(Eight);
			ButtonX2.CalculatorTask.Name = "9";
			ButtonX2.CalculatorTask.Add(Nine);

			ButtonY0.CalculatorTask.Name = "4";
			ButtonY0.CalculatorTask.Add(Four);
			ButtonY1.CalculatorTask.Name = "5";
			ButtonY1.CalculatorTask.Add(Five);
			ButtonY2.CalculatorTask.Name = "6";
			ButtonY2.CalculatorTask.Add(Six);

			ButtonY3.CalculatorTask.Name = "×";
			ButtonY3.CalculatorTask.Add(Multiplication);
			ButtonY4.CalculatorTask.Name = "÷";
			ButtonY4.CalculatorTask.Add(Division);

			ButtonZ0.CalculatorTask.Name = "1";
			ButtonZ0.CalculatorTask.Add(One);
			ButtonZ1.CalculatorTask.Name = "2";
			ButtonZ1.CalculatorTask.Add(Two);
			ButtonZ2.CalculatorTask.Name = "3";
			ButtonZ2.CalculatorTask.Add(Three);

			ButtonZ3.CalculatorTask.Name = "+";
			ButtonZ3.CalculatorTask.Add(Addition);
			ButtonZ4.CalculatorTask.Name = "-";
			ButtonZ4.CalculatorTask.Add(Subtraction);

			ButtonW0.CalculatorTask.Name = "0";
			ButtonW0.CalculatorTask.Add(Zero);
			ButtonW1.CalculatorTask.Name = ".";
			ButtonW1.CalculatorTask.Add(Definitions.OperationDefinitions.Point);

			Evaluate.Rename("=");
			Answer.Rename("Ans");
			Shift.Rename("Shift");
			Backspace.Rename("C");
			AllClear.Rename("AC");
		}

		public void PrimaryButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is SingleTaskButton)
			{
				WriteTokenToScreen?.Invoke(sender, EventArgs.Empty);
			}
		}

		public void ToggleShift_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "Shift")
			{
				ToggleShiftables?.Invoke();
			}
		}

		public void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "Backspace")
			{
				BackspaceToken?.Invoke();
			}
		}

		public void ClearAll_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "AllClear")
			{
				ClearEntireExpression?.Invoke();
			}
		}

		public void EvaluateExpression_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "Evaluate")
			{
				EvaluateExpression?.Invoke();
			}
		}

		public void GetLastAnswer_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "Answer")
			{
				GetLastAnswer?.Invoke();
			}
		}
	}
}
