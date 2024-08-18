using MultiCalculator.Abstractions;
using MultiCalculator.Delegates;
using MultiCalculator.Models;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for CalculatorControlBar.xaml
	/// </summary>
	public partial class CalculatorControlBar : UserControl
	{
		public event SimpleEventHandler? CopyToClipBoard;

		public CalculatorControlBar()
		{
			InitializeComponent();
			DataContext = new ComboBoxModel();
		}

		void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var comboBox = sender as ComboBox;
			if (comboBox!.SelectedItem is string selectedItem)
			{
				var selectedPackage = PluginManager.Instance.PluginPackages.FirstOrDefault(x => x.Name == selectedItem);
				if (selectedPackage == null)
				{
					MessageBox.Show($"Could not find package: {selectedItem}");
				}
				else
				{
					BuildWindow(selectedPackage);
				}
			}
		}

		void Copy_Click(object sender, RoutedEventArgs e)
		{
			CopyToClipBoard?.Invoke();
		}

		void BuildWindow(PluginPackage package)
		{
			var packageWindow = new PackageWindow();
			packageWindow.Build(package, this);
			packageWindow.Show();
        }

		public void CalculatorSubscribesToButton(Button button, IToken t)
		{
			var parentWindow = (ScientificCalculatorWindow)Window.GetWindow(this);
			if (parentWindow is null)
			{
				MessageBox.Show($"package not associated with calculator");
			}
			else
			{
				parentWindow.SubscribeToCustomButtons(button, t);
			}
		}
	}
}
