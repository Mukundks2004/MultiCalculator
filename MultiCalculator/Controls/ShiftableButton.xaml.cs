using MultiCalculator.Abstractions;
using MultiCalculator.Implementations;
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

			Token = new UnaryOperationToken();
			SecondaryButtonOperation = new UnaryOperationToken() { DisplayName = "Sec" };
			DataContext = this;
		}

		public void Toggle()
		{
			(Token, SecondaryButtonOperation) = (SecondaryButtonOperation, Token);
		}

		public static readonly DependencyProperty ButtonOperationProperty = DependencyProperty.Register("ButtonOperation", typeof(IToken), typeof(ShiftableButton), new PropertyMetadata(new UnaryOperationToken(), ValueChanged));
		
		public static readonly DependencyProperty SecondaryButtonOperationProperty = DependencyProperty.Register("SecondaryButtonOperation", typeof(IToken), typeof(ShiftableButton), new PropertyMetadata(new UnaryOperationToken(), ValueChangedSecondary));

		public IToken Token
		{
			get => (IToken)GetValue(ButtonOperationProperty);
			set => SetValue(ButtonOperationProperty, value);
		}

		public IToken SecondaryButtonOperation
		{
			get => (IToken)GetValue(SecondaryButtonOperationProperty);
			set => SetValue(SecondaryButtonOperationProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Primary.Content = ((IToken)e.NewValue).DisplayName;
		}

		static void ValueChangedSecondary(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Secondary.Content = ((IToken)e.NewValue).DisplayName;
		}
	}
}
