using Microsoft.EntityFrameworkCore.Query.Internal;
using MultiCalculator.Abstractions;
using MultiCalculator.Controls;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using MultiCalculator.Delegates;
using MultiCalculator.Implementations;
using MultiCalculator.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for ScientificCalculatorWindow.xaml
	/// </summary>
	public partial class ScientificCalculatorWindow : Window
	{
		readonly IDatabaseService _databaseService;
		readonly UserModel _user;

		public ScientificCalculatorWindow(IDatabaseService databaseService, UserModel user)
		{
			InitializeComponent();
			Icon = new BitmapImage(new Uri("pack://application:,,,./icon.png"));
			_databaseService = databaseService;
			_user = user;
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
		
		public static readonly DependencyProperty CalculatorAnswerProperty = DependencyProperty.Register("CalculatorAnswer", typeof(string), typeof(ScientificCalculatorWindow), new PropertyMetadata(string.Empty, AnswerValueChanged));

		public TokenChain CalculatorExpression
		{
			get => (TokenChain)GetValue(CalculatorExpressionProperty);
			set => SetValue(CalculatorExpressionProperty, value);
		}

		public string CalculatorAnswer
		{
			get => (string)GetValue(CalculatorAnswerProperty);
			set => SetValue(CalculatorAnswerProperty, value);
		}

		public void SubscribeToCustomButtons(Button button, IToken t)
		{
			button.Click += (sender, e) => CustomButton_Clicked(t);
		}

		public void UpdateExpressionBox()
		{
			try
			{
				var isValid = CalculatorExpression.IsValid();
				if (isValid)
				{
					ExpressionBoxContainer.Visibility = Visibility.Collapsed;
					FormulaBoxContainer.Visibility = Visibility.Visible;
					FormulaBox.Formula = CalculatorExpression.GetLatexString();
				}
				else
				{
					ExpressionBoxContainer.Visibility = Visibility.Visible;
					FormulaBoxContainer.Visibility = Visibility.Collapsed;
					ExpressionBox.Text = CalculatorExpression.ToString();
				}
			}
			catch
			{
				ExpressionBoxContainer.Visibility = Visibility.Visible;
				FormulaBoxContainer.Visibility = Visibility.Collapsed;
				ExpressionBox.Text = CalculatorExpression.ToString();
			}
		}

		static void ExpressionValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ScientificCalculatorWindow;
			_ = control ?? throw new ArgumentNullException(nameof(control));
			control.UpdateExpressionBox();
		}

		static void AnswerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var control = d as ScientificCalculatorWindow;
			_ = control ?? throw new ArgumentNullException(nameof(control));

			//Need to change this to something else when ans box made
			//control.ResultBox.Text = e.NewValue.ToString();
			control.FormulaResult.Formula = (string)e.NewValue;
		}

		void TokenButton_Clicked(object? sender, EventArgs e)
		{
			var button = sender as ICalculatorButton;
			_ = button ?? throw new ArgumentNullException(nameof(button));
			CalculatorAnswer = string.Empty;
			CalculatorExpression.Add(button.CalculatorTask.Tokens);
		}

        void StoreAnswer(string question, string answer)
		{
			_databaseService.AddCalculationHistory(new CalculationHistoryModel { Id = new Guid(), Question = question, Answer = answer, QuestionSender = _user });
		}

		void CustomButton_Clicked(IToken? t)
		{
			if (t != null)
			{
				CalculatorAnswer = string.Empty;
				CalculatorExpression.Add(t);
			}
		}

		public void GetAnswer_Click()
		{
			CalculatorAnswer = string.Empty;
			//Replace this with real answer when we have a history service or something- maybe we need a history model?
			//Also move explicit references to token to service maybe. the view should not know about the impelemtnation details.
			CalculatorExpression.Add(new NullaryOperationToken() { Calculate = () => 0, TokenSymbol = "Ans", LatexString = "{Ans}"});
		}

		public void EvaluateExpression_Click()
		{
			try
			{
				var isValid = CalculatorExpression.IsValid();
				if (!isValid)
				{
					CalculatorAnswer = "Syntax Error";
				}
				else
				{
					CalculatorExpression.InsertMultiplicationSignsConvertUnaryDualsToUnaryPlaceBrackets();
					var result = CalculatorExpression.ParseTree();
					CalculatorAnswer = double.IsNaN(result) || result == double.PositiveInfinity || result == double.NegativeInfinity ? "Math Error" : result.ToString();
					StoreAnswer(CalculatorExpression.GetLatexString(), result.ToString());
				}
			}
			catch
			{
				CalculatorAnswer = "Bad expression!";
			}

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
			CalculatorAnswer = string.Empty;
			CalculatorExpression.MakeEmpty();
		}

		public void MoveLeft_Click(object sender, RoutedEventArgs e)
		{
			CalculatorExpression.MoveCursorLeft();
		}

		public void MoveRight_Click(object sender, RoutedEventArgs e)
		{
			CalculatorExpression.MoveCursorRight();
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
