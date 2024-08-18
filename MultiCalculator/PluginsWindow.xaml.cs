using Microsoft.Win32;
using MultiCalculator.Abstractions;
using MultiCalculator.Exceptions;
using MultiCalculator.Implementations;
using MultiCalculator.Models;
using MultiCalculator.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for PluginsWindow.xaml
	/// </summary>
	public partial class PluginsWindow : Window
	{
		const int OutOfBoundsHeight = 350;
		const int OutOfBoundsWidth = 300;

		readonly PluginPackage package = new();
		public PluginsWindow()
		{
			InitializeComponent();
		}

		void UploadButton_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog();
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

				var allPluginsInDll = PluginLoaderService.LoadPluginsFromFile(FilePathTextBox.Text);
				var matchingAbstraction = allPluginsInDll.FirstOrDefault(r => r.GetType().ToString().EndsWith(CustomName.Text)) ?? throw new MultiCalculatorException($"name {CustomName.Text} not found in dll");
				
				IToken matchingImplementation;
                if (matchingAbstraction is IUnaryOperation unary)
                {
					matchingImplementation = new UnaryOperationToken()
					{
						CalculateUnary = unary.CalculateUnary,
						LatexString = unary.LatexString,
						TokenSymbol = unary.TokenSymbol,
						Fixity = unary.Fixity,
						Priority = unary.Priority,
					};
                }
				else if (matchingAbstraction is INullaryOperation constant)
				{
					matchingImplementation = new NullaryOperationToken()
					{
						TokenSymbol = constant.TokenSymbol,
						LatexString = constant.LatexString,
						Calculate = constant.Calculate,
					};
				}
				else if (matchingAbstraction is IBinaryOperation binary)
				{
					matchingImplementation = new BinaryOperationToken()
					{
						TokenSymbol = binary.TokenSymbol,
						LatexString = binary.LatexString,
						Priority = binary.Priority,
						Associativity = binary.Associativity,
						CalculateBinary = binary.CalculateBinary,
					};
				}
				else
				{
					throw new MultiCalculatorException("trouble loading dll itoken");
				}

                var newButton = new PluginButton()
				{
					XPos = inputXPos,
					YPos = inputYPos,
					Width = inputWidth,
					Height = inputHeight,
					DllPath = FilePathTextBox.Text,
					Name = CustomName.Text,
					Token = matchingImplementation,
				};

				package.Buttons.Add(newButton);

				var btn = new Button
				{
					Content = matchingImplementation.TokenSymbol,
					Width = inputWidth,
					Height = inputHeight,
					FontFamily = new System.Windows.Media.FontFamily("MS Gothic"),
					FontSize = 12,
					FontWeight = FontWeights.Bold,
				};

				Canvas.SetLeft(btn, newButton.XPos);
				Canvas.SetTop(btn, newButton.YPos);
				Sandbox.Children.Add(btn);

				XPosBox.Text = string.Empty;
				YPosBox.Text = string.Empty;
				CustomHeightBox.Text = string.Empty;
				CustomWidthBox.Text = string.Empty;
				CustomName.Text = string.Empty;
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
			try
			{
				if (PackageNameBox.Text == string.Empty)
				{
					throw new MultiCalculatorException("enter package name first");
				}

				package.Name = PackageNameBox.Text;
				PluginManager.Instance.PluginPackages.Add(package);
				Close();
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
