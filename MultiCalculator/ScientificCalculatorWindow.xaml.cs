using MultiCalculator.Abstractions;
using MultiCalculator.Controls;
using MultiCalculator.Implementations;
using MultiCalculator.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
			CalculatorExpression.OperationsUpdated += UpdateExpressionBox;
			DataContext = this;

			ScientificButtonsGrid.WriteTokenToScreen += TokenButton_Clicked;

			PrimaryButtonsGrid.WriteTokenToScreen += TokenButton_Clicked;
			PrimaryButtonsGrid.ToggleShiftables += ToggleShift_Click;
			PrimaryButtonsGrid.GetLastAnswer += GetAnswer_Click;
			PrimaryButtonsGrid.BackspaceToken += Delete_Click;
			PrimaryButtonsGrid.ClearEntireExpression += ClearAll_Click;
			PrimaryButtonsGrid.EvaluateExpression += EvaluateExpression_Click;
		}

		public static readonly DependencyProperty CalculatorExpressionProperty = DependencyProperty.Register("CalculatorExpression", typeof(TokenChain), typeof(ScientificCalculatorWindow), new PropertyMetadata(null, ExpressionValueChanged));

		public static readonly DependencyProperty CalculatorAnswerProperty = DependencyProperty.Register("CalculatorAnswer", typeof(double), typeof(ScientificCalculatorWindow), new PropertyMetadata((double)0, AnswerValueChanged));

		public TokenChain CalculatorExpression
		{
			get => (TokenChain)GetValue(CalculatorExpressionProperty);
			set => SetValue(CalculatorExpressionProperty, value);
		}

		public double CalculatorAnswer
		{
			get => (double)GetValue(CalculatorAnswerProperty);
			set => SetValue(CalculatorAnswerProperty, value);
		}

		public void UpdateExpressionBox()
		{
			ExpressionBox.Text = CalculatorExpression.ToString();
		}

		static void ExpressionValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ScientificCalculatorWindow;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.ExpressionBox.Text = ((TokenChain)e.NewValue).ToString();
		}

		static void AnswerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ScientificCalculatorWindow;
			_ = control ?? throw new ArgumentNullException(nameof(control));

			//Need to change this to something else when ans box made
			control.ResultBox.Text = e.NewValue.ToString();
		}

		void TokenButton_Clicked(object? sender, EventArgs e)
		{
			var button = sender as ICalculatorButton;
			_ = button ?? throw new ArgumentNullException(nameof(button));
			CalculatorExpression.Add(button.Token);
		}

		public void GetAnswer_Click()
		{
			//Replace this with real answer when we have a history service or something- maybe we need a history model?
			//Also move explicit references to token to service maybe. the view should not know about the impelemtnation details.
			CalculatorExpression.Add(BracketToken.ClosedBracket);
			CalculatorExpression.Add(NullaryOperationToken.GetNullaryOperationFromDouble());
			CalculatorExpression.Add(BracketToken.OpenBracket);
		}

		public void EvaluateExpression_Click()
		{
			var result = CalculatorExpression.Parse();
			CalculatorAnswer = result;
			CalculatorExpression.MakeEmpty();
		}

		public void ToggleShift_Click()
		{
			ToggleAllButtons(this);
		}

		public void Delete_Click()
		{
			CalculatorExpression.RemoveLastBeforeCursor();
		}

		public void ClearAll_Click()
		{
			CalculatorExpression = new TokenChain();
		}

		static void ToggleAllButtons(DependencyObject parent)
		{
			int childCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (child is ShiftableButton shiftableButton)
				{
					shiftableButton.Toggle();
				}
				else
				{
					ToggleAllButtons(child);
				}
			}
		}
	}
}
