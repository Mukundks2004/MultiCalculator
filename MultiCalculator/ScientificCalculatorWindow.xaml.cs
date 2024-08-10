using MultiCalculator.Abstractions;
using MultiCalculator.Controls;
using MultiCalculator.Implementations;
using MultiCalculator.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Interaction logic for ScientificCalculatorWindow.xaml
	/// </summary>
	public partial class ScientificCalculatorWindow : Window
	{
		public ScientificCalculatorWindow()
		{
			InitializeComponent();
			CalculatorExpression = new TokenChain();
			DataContext = this;
		}

		public static readonly DependencyProperty CalculatorExpressionProperty = DependencyProperty.Register("CalculatorExpression", typeof(TokenChain), typeof(ScientificCalculatorWindow), new PropertyMetadata(new TokenChain(), ValueChanged));

		public TokenChain CalculatorExpression
		{
			get => (TokenChain)GetValue(CalculatorExpressionProperty);
			set => SetValue(CalculatorExpressionProperty, value);
		}

		static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ScientificCalculatorWindow;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.ResultBox.Text = ((TokenChain)e.NewValue).ToString();
		}
	}
}
