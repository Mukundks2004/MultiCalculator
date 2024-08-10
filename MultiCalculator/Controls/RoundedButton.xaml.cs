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
			ButtonOperation = new UnaryOperationToken() { DisplayName = "S" };
			DataContext = this;
        }

		public static readonly DependencyProperty ButtonOperationProperty = DependencyProperty.Register("ButtonOperation", typeof(IToken), typeof(RoundedButton), new PropertyMetadata(new UnaryOperationToken(), ValueChanged));

		public IToken ButtonOperation
		{
			get => (IToken)GetValue(ButtonOperationProperty);
			set => SetValue(ButtonOperationProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as RoundedButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Button.Content = ((IToken)e.NewValue).DisplayName;
		}
	}
}
