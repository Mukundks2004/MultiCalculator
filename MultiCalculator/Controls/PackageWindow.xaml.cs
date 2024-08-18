using MultiCalculator.Abstractions;
using MultiCalculator.Delegates;
using MultiCalculator.Models;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for PackageWindow.xaml
	/// </summary>
	public partial class PackageWindow : Window
	{
		public event EventHandlerWithToken? WriteCustomTokenToScreen;

		public PackageWindow()
		{
			InitializeComponent();
		}

		public void Build(PluginPackage package, CalculatorControlBar controllingWindow)
		{
			foreach (var plugin in package.Buttons)
			{
				var btn = new Button
				{
					Content = plugin.Name,
					Width = plugin.Width,
					Height = plugin.Height
				};

				Canvas.SetLeft(btn, plugin.XPos);
				Canvas.SetTop(btn, plugin.YPos);
				Sandbox.Children.Add(btn);

				if (plugin.Token is not null)
				{
					controllingWindow.CalculatorSubscribesToButton(btn, plugin.Token);
				}
			}
		}

		//public void CustomButton_Click(object sender, RoutedEventArgs e, IToken? t)
		//{
		//	WriteCustomTokenToScreen?.Invoke(sender, e, t);
		//}
	}
}
