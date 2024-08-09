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

			ButtonOperation = new UnaryButtonOperation();
			SecondaryButtonOperation = new UnaryButtonOperation() { DisplayName = "Sec" };
			DataContext = this;
		}

		public void Toggle()
		{
			(ButtonOperation, SecondaryButtonOperation) = (SecondaryButtonOperation, ButtonOperation);
		}

		public static readonly DependencyProperty ButtonOperationProperty = DependencyProperty.Register("ButtonOperation", typeof(IButtonOperation), typeof(ShiftableButton), new PropertyMetadata(new UnaryButtonOperation(), ValueChanged));
		
		public static readonly DependencyProperty SecondaryButtonOperationProperty = DependencyProperty.Register("SecondaryButtonOperation", typeof(IButtonOperation), typeof(ShiftableButton), new PropertyMetadata(new UnaryButtonOperation(), ValueChangedSecondary));

		public IButtonOperation ButtonOperation
		{
			get => (IButtonOperation)GetValue(ButtonOperationProperty);
			set => SetValue(ButtonOperationProperty, value);
		}

		public IButtonOperation SecondaryButtonOperation
		{
			get => (IButtonOperation)GetValue(SecondaryButtonOperationProperty);
			set => SetValue(SecondaryButtonOperationProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Primary.Content = ((IButtonOperation)e.NewValue).DisplayName;
		}

		static void ValueChangedSecondary(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ShiftableButton;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.Secondary.Content = ((IButtonOperation)e.NewValue).DisplayName;
		}
	}
}
