using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MultiCalculator.Database;
using MultiCalculator.Extensions;
using MultiCalculator.Database.Services;
using MultiCalculator.Database.Repositories;
using MultiCalculator.Database.Models;
using MultiCalculator.Helpers;

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

            var builder = Host.CreateApplicationBuilder();
            builder.Services.AddDependencyGroup();

            using (var context = new CalculatorDbContext())
            {
                context.Database.EnsureCreated();
            }
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