using MultiCalculator.Controls;
using MultiCalculator.Exceptions;
using MultiCalculator.Implementations;
using System.Windows;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for ScientificFunctionsWindow.xaml
	/// </summary>
	public partial class ScientificFunctionsWindow : Window
	{
		public ScientificFunctionsWindow()
		{
			InitializeComponent();
		}

		public void ButtonToggle_Click(object sender, RoutedEventArgs e)
		{
			var scientificBox = (ScientificFunctions)FindName("gridA");
			scientificBox.ToggleAllButtons();
		}
	}
}
