using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator.Controls
{
	/// <summary>
	/// Interaction logic for SingleTokenButton.xaml
	/// </summary>
	public partial class SingleTokenButton : UserControl, ICalculatorButton
	{
        public SingleTokenButton()
        {
            InitializeComponent();
			Token = new UnaryOperationToken() { DisplayName = "S" };
			DataContext = this;
        }

		public static readonly DependencyProperty ButtonOperationProperty = DependencyProperty.Register("ButtonOperation", typeof(IToken), typeof(SingleTokenButton), new PropertyMetadata(new UnaryOperationToken(), ValueChanged));

		public IToken Token
		{
			get => (IToken)GetValue(ButtonOperationProperty);
			set => SetValue(ButtonOperationProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as SingleTokenButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Button.Content = ((IToken)e.NewValue).DisplayName;
		}
	}
}
