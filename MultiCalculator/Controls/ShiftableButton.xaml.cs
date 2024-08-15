using MultiCalculator.Abstractions;
using MultiCalculator.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for ShiftableButton.xaml
	/// </summary>
	public partial class ShiftableButton : UserControl, ICalculatorButton
	{
		public ShiftableButton()
		{
			InitializeComponent();

			CalculatorTask = new CalculatorTask();
			SecondaryCalculatorTask = new CalculatorTask();
			DataContext = this;

			CalculatorTask.TokensUpdated += UpdateButtonText;
			SecondaryCalculatorTask.TokensUpdated += UpdateButtonText;
		}

		public void Toggle()
		{
			(CalculatorTask, SecondaryCalculatorTask) = (SecondaryCalculatorTask, CalculatorTask);
			UpdateButtonText();
		}

		public static readonly DependencyProperty ButtonNameProperty = DependencyProperty.Register("ButtonName", typeof(string), typeof(ShiftableButton), new PropertyMetadata(string.Empty, ButtonTextChanged));

		public static readonly DependencyProperty SecondaryButtonNameProperty = DependencyProperty.Register("SecondaryButtonName", typeof(string), typeof(ShiftableButton), new PropertyMetadata(string.Empty, SecondaryButtonTextChanged));

		public CalculatorTask CalculatorTask { get; set; }

		public CalculatorTask SecondaryCalculatorTask { get; set; }

		public string ButtonName
		{
			get => (string)GetValue(ButtonNameProperty);
			set => SetValue(ButtonNameProperty, value);
		}

		public string SecondaryButtonName
		{
			get => (string)GetValue(SecondaryButtonNameProperty);
			set => SetValue(SecondaryButtonNameProperty, value);
		}

		static void ButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Primary.Content = (string)e.NewValue;
		}

		static void SecondaryButtonTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Secondary.Content = (string)e.NewValue;
		}

		void UpdateButtonText()
		{
			ButtonName = CalculatorTask.Name;
			SecondaryButtonName = SecondaryCalculatorTask.Name;
		}
	}
}
