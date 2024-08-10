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
		public PrimaryFunctions()
		{
			InitializeComponent();

			ButtonX0.ButtonOperation = new DigitToken() { DisplayName = "7" };
			ButtonX1.ButtonOperation = new DigitToken() { DisplayName = "8" };
			ButtonX2.ButtonOperation = new DigitToken() { DisplayName = "9" };

			ButtonY0.ButtonOperation = new DigitToken() { DisplayName = "4" };
			ButtonY1.ButtonOperation = new DigitToken() { DisplayName = "5" };
			ButtonY2.ButtonOperation = new DigitToken() { DisplayName = "6" };
			ButtonY3.ButtonOperation = new BinaryOperationToken() { DisplayName = "×", CalculateBinary = (x, y) => x * y };
			ButtonY4.ButtonOperation = new BinaryOperationToken() { DisplayName = "÷", CalculateBinary = (x, y) => x / y };


			ButtonZ0.ButtonOperation = new DigitToken() { DisplayName = "1" };
			ButtonZ1.ButtonOperation = new DigitToken() { DisplayName = "2" };
			ButtonZ2.ButtonOperation = new DigitToken() { DisplayName = "3" };
			ButtonZ3.ButtonOperation = new DualArityOperationToken() { DisplayName = "+", CalculateBinary = (x, y) => x + y, CalculateUnary = (x) => x };
			ButtonZ4.ButtonOperation = new DualArityOperationToken() { DisplayName = "-", CalculateBinary = (x, y) => x - y, CalculateUnary = (x) => -x };

			ButtonW0.ButtonOperation = new DigitToken() { DisplayName = "0" };
			ButtonW1.ButtonOperation = new DigitToken() { DisplayName = "." };
		}

		public void PrimaryButton_Click(object sender, RoutedEventArgs e)
		{
		}

		public void GetAnswer_Click(object sender, RoutedEventArgs e)
		{
		}

		public void EvaluateExpression_Click(object sender, RoutedEventArgs e)
		{
		}

		public void ToggleShift_Click(object sender, RoutedEventArgs e)
		{
		}

		public void Delete_Click(object sender, RoutedEventArgs e)
		{
		}

		public void ClearAll_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}
