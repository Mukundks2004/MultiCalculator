using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Windows.Controls.Primitives;
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
		const int OutOfBoundsHeight = 350;
		const int OutOfBoundsWidth = 300;

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
				var inputXPos = int.Parse(XPosBox.Text);
				var inputYPos = int.Parse(YPosBox.Text);
				var inputHeight = int.Parse(CustomHeightBox.Text);
				var inputWidth = int.Parse(CustomWidthBox.Text);

				if (inputWidth < 30 || inputHeight < 30)
				{
					throw new MultiCalculatorException("button too small");
				}
				if (inputXPos + inputWidth < 0 || inputXPos + inputWidth > OutOfBoundsWidth || inputYPos + inputHeight > OutOfBoundsHeight || inputYPos + inputHeight < 0)
				{
					throw new MultiCalculatorException("button out of range");
				}
				if (FilePathTextBox.Text == string.Empty || CustomName.Text == string.Empty)
				{
					throw new MultiCalculatorException("dll missing/name invalid");
				}
				if (PackageNameBox.Text == string.Empty)
				{
					throw new MultiCalculatorException("enter package name first");
				}

				var newButtonDetails = new PluginButton()
				{
					XPos = inputXPos,
					YPos = inputYPos,
					Width = inputWidth,
					Height = inputHeight,
					DllPath = FilePathTextBox.Text,
					Name = CustomName.Text
				};

				pluginPaths.Add(newButtonDetails);

				var btn = new Button
				{
					Content = CustomName.Text,
					Width = inputWidth,
					Height = inputHeight
				};

				Canvas.SetLeft(btn, newButtonDetails.XPos);
				Canvas.SetTop(btn, newButtonDetails.YPos);
				Sandbox.Children.Add(btn);

				XPosBox.Text = string.Empty;
				YPosBox.Text = string.Empty;
				CustomHeightBox.Text = string.Empty;
				CustomWidthBox.Text = string.Empty;
				FilePathTextBox.Text = string.Empty;
			}
			catch (Exception ex)
			{
				var messageBoxText = ex.Message;
				var caption = "MultiCalculatorWarning";
				var button = MessageBoxButton.OK;
				var icon = MessageBoxImage.Warning;

				MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
			}
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

		void SaveAndQuit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		void QuitWithoutSaving_Click(object sender, RoutedEventArgs e)
		{
			Close();
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
