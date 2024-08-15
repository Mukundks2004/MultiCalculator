using MultiCalculator.Abstractions;
using MultiCalculator.Delegates;
using MultiCalculator.Implementations;
using MultiCalculator.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for SingleTokenButton.xaml
	/// </summary>
	public partial class SingleTaskButton : UserControl, ICalculatorButton
	{
		public SingleTaskButton()
        {
            InitializeComponent();
			CalculatorTask = new CalculatorTask();
			CalculatorTask.TokensUpdated += UpdateButtonName;
			DataContext = this;
        }
		
		public static readonly DependencyProperty ButtonNameProperty = DependencyProperty.Register("ButtonName", typeof(string), typeof(SingleTaskButton), new PropertyMetadata(string.Empty, ButtonNameChanged));
		
		public CalculatorTask CalculatorTask { get; set; }

		public string ButtonName
		{
			get => (string)GetValue(ButtonNameProperty);
			set => SetValue(ButtonNameProperty, value);
		}

		static void ButtonNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as SingleTaskButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Button.Content = (string)e.NewValue;
		}

		void UpdateButtonName()
		{
			ButtonName = CalculatorTask.Name;
		}
	}
}
