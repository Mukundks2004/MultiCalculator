using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void OpenWindowButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button)
			{
				var tag = button.Tag as string;
				OpenWindow(tag);
			}
		}

		void OpenWindow(string? windowTag)
		{
			Window? windowToOpen = null;

			switch (windowTag)
			{
				case "ScientificCalculator":
					windowToOpen = new ScientificCalculatorWindow();
					break;
				default:
					MessageBox.Show($"Unknown window tag: {windowTag ?? "Empty"}");
					break;
			}

			if (windowToOpen != null)
			{
				windowToOpen.Show();
			}
		}
	}
}