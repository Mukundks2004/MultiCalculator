using System.Windows;
using System.Windows.Media.Imaging;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using MultiCalculator.Helpers;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for PracticeProblems.xaml
	/// </summary>
	public partial class PracticeProblemsWindow : Window
	{
		private string question;
		private string answer;
		readonly IDatabaseService _databaseService;
		readonly UserModel _user;
		PracticeProblemsHelper problemHelper;
		PdfWriterHelper pdfWriterHelper;

		public PracticeProblemsWindow(IDatabaseService databaseService, UserModel user)
		{
			InitializeComponent();
			Icon = new BitmapImage(new Uri("pack://application:,,,./icon.png"));
            problemHelper = new PracticeProblemsHelper(databaseService);
			pdfWriterHelper = new PdfWriterHelper();
            _databaseService = databaseService;
			_user = user;
		}

		void GeneratePracticeProblem_Click(object sender, RoutedEventArgs e)
		{
			(question, answer) = problemHelper.GeneratePracticeProblem();
			QuestionTextBlock.Text = question;
			answerText.Text = string.Empty;
			answerBtn.Content = "Show Answer";
		}

        void HandleAnswer_Click(object sender, RoutedEventArgs e)
        {
			if (question != null && question != string.Empty)
			{
				if (answerText.Text == string.Empty)
				{
					answerText.Text = "Answer: " + answer;
					answerText.Visibility = Visibility.Visible;
					answerBtn.Content = "Hide Answer";
				}
				else
				{

                    answerText.Text = string.Empty;
                    answerBtn.Content = "Show Answer";
                }
			}
        }

        void SendEmail_Click(object sender, RoutedEventArgs e)
        {
			if (QuestionTextBlock.Text != string.Empty && QuestionTextBlock.Text != null && !QuestionTextBlock.Text.Equals("Here will be the generated question"))
			{
				var email = DetermineEmail();
				problemHelper.SendPracticeProblemEmail(_user, QuestionTextBlock.Text, string.IsNullOrWhiteSpace(email) ? null : email);
				MessageBox.Show("Email has been sent!");
            }
			else
			{
				MessageBox.Show("Please request for a question prior to sending an email.");
			}
        }

        private void GeneratePdfOfQuestions_Click(object sender, RoutedEventArgs e)
        {
			var numberOfQuestionsText = numberOfQuestionTextBox.Text;
			int numberOfQuestions; 
			try
			{
				numberOfQuestions = Int32.Parse(numberOfQuestionsText);
			}
			catch
			{
				MessageBox.Show("Please enter an integer amount.");
				return;
			}
			var problems = problemHelper.GenerateNAmountOfPracticeProblems(numberOfQuestions);
			pdfWriterHelper.GenerateQuestionSheet(problems, _user, sendToEmail: true, email: DetermineEmail());
            MessageBox.Show("Email has been sent!");
        }

		string DetermineEmail()
		{
			bool customEmailCheck = CustomEmailRadioButton.IsChecked ?? false;
			if (customEmailCheck)
			{
				if (EmailTextBox.Text == null || EmailTextBox.Text.Equals(string.Empty))
				{
                    MessageBox.Show("Please have an email selected if this option is chosen.");
				}
                else
                {
					return EmailTextBox.Text;
                }
            }
			else
			{
				return _user.Email;
			}
			return string.Empty;
        }

        // AccountEmailRadioButton
    }
}
