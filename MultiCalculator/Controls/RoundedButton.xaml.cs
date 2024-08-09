using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for RoundedButton.xaml
	/// </summary>
	public partial class RoundedButton : UserControl, ICalculatorButton
	{
        public RoundedButton()
        {
            InitializeComponent();
			ButtonOperation = new UnaryButtonOperation() { DisplayName = "Single" };
			DataContext = this;
        }

		public static readonly DependencyProperty ButtonOperationProperty = DependencyProperty.Register("ButtonOperation", typeof(IButtonOperation), typeof(RoundedButton), new PropertyMetadata(new UnaryButtonOperation(), ValueChanged));

		public IButtonOperation ButtonOperation
		{
			get => (IButtonOperation)GetValue(ButtonOperationProperty);
			set => SetValue(ButtonOperationProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as RoundedButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Button.Content = ((IButtonOperation)e.NewValue).DisplayName;
		}
	}
}
