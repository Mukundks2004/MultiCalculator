using Microsoft.Win32;
using MultiCalculator.Exceptions;
using MultiCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for PluginsWindow.xaml
	/// </summary>
	public partial class PluginsWindow : Window
	{
		const int MaxHeight = 350;
		const int MaxWidth = 300;

		readonly List<PluginButton> pluginPaths = [];
		public PluginsWindow()
		{
			InitializeComponent();
		}

		void UploadButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*";
			openFileDialog.Title = "Select a DLL file";

			if (openFileDialog.ShowDialog() == true)
			{
				FilePathTextBox.Text = openFileDialog.FileName;
			}
		}

		void AddPlugin_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (int.Parse(XPosBox.Text + 40) > MaxWidth || int.Parse(YPosBox.Text + 40) > MaxHeight)
				{
					throw new MultiCalculatorException("out of range");
				}

				var newButtonDetails = new PluginButton()
				{
					XPos = int.Parse(XPosBox.Text),
					YPos = int.Parse(YPosBox.Text),
					Width = 40,
					Height = 40,
					DllPath = FilePathTextBox.Text,
				};

				pluginPaths.Add(newButtonDetails);
			}
			catch
			{
			}

			XPosBox.Text = string.Empty;
			YPosBox.Text = string.Empty;
			FilePathTextBox.Text = string.Empty;
		}

		void PositionBoxes_PreviewInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		void PositionBoxes_Pasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string text = (string)e.DataObject.GetData(typeof(string));
				if (!IsTextAllowed(text))
				{
					e.CancelCommand();
				}
			}
			else
			{
				e.CancelCommand();
			}
		}

		static bool IsTextAllowed(string text)
		{
			Regex regex = NumberRegex();
			return !regex.IsMatch(text);
		}

		[GeneratedRegex("[^0-9]+")]
		private static partial Regex NumberRegex();
	}
}
