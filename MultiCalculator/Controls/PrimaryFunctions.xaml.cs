using MultiCalculator.Delegates;
using MultiCalculator.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

			ButtonX0.Token = new DigitToken() { DisplayName = "7" };
			ButtonX1.Token = new DigitToken() { DisplayName = "8" };
			ButtonX2.Token = new DigitToken() { DisplayName = "9" };

			ButtonY0.Token = new DigitToken() { DisplayName = "4" };
			ButtonY1.Token = new DigitToken() { DisplayName = "5" };
			ButtonY2.Token = new DigitToken() { DisplayName = "6" };

			ButtonY3.Token = new BinaryOperationToken() { DisplayName = "×", CalculateBinary = (x, y) => x * y };
			ButtonY4.Token = new BinaryOperationToken() { DisplayName = "÷", CalculateBinary = (x, y) => x / y };

			ButtonZ0.Token = new DigitToken() { DisplayName = "1" };
			ButtonZ1.Token = new DigitToken() { DisplayName = "2" };
			ButtonZ2.Token = new DigitToken() { DisplayName = "3" };
			ButtonZ3.Token = new DualArityOperationToken() { DisplayName = "+", CalculateBinary = (x, y) => x + y, CalculateUnary = (x) => x };
			ButtonZ4.Token = new DualArityOperationToken() { DisplayName = "-", CalculateBinary = (x, y) => x - y, CalculateUnary = (x) => -x };

			ButtonW0.Token = new DigitToken() { DisplayName = "0" };
			ButtonW1.Token = new DigitToken() { DisplayName = "." };

			Equals.Rename("=");
			Answer.Rename("Ans");
			Shift.Rename("Shift");
			Backspace.Rename("C");
			AllClear.Rename("AC");
		}

		public void PrimaryButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is SingleTokenButton)
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
			if (sender is RoundedButton button && button.Name == "C")
			{
				BackspaceToken?.Invoke();
			}
		}

		public void ClearAll_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "AC")
			{
				ClearEntireExpression?.Invoke();
			}
		}

		public void EvaluateExpression_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "=")
			{
				EvaluateExpression?.Invoke();
			}
		}

		public void GetLastAnswer_Click(object sender, RoutedEventArgs e)
		{
			if (sender is RoundedButton button && button.Name == "Ans")
			{
				GetLastAnswer?.Invoke();
			}
		}
	}
}
