using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Repositories;
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
		readonly UserModel user;
		PracticeProblemsHelper helper;

		public PracticeProblemsWindow(IDatabaseService databaseService, UserModel user)
		{
			InitializeComponent();
			helper = new PracticeProblemsHelper(databaseService);
			_databaseService = databaseService;
			user = user;
		}

		void GeneratePracticeProblem_Click(object sender, RoutedEventArgs e)
		{
			(question, answer) = helper.GeneratePracticeProblem();
			QuestionTextBlock.Text = question;
			answerLabel.Content = string.Empty;
			answerBtn.Content = "Show Answer";
		}

        void HandleAnswer_Click(object sender, RoutedEventArgs e)
        {
			if (question != null && question != string.Empty)
			{
				if (answerLabel.Content == string.Empty)
				{
					answerLabel.Content = "Answer: " + answer;
					answerBtn.Content = "Hide Answer";
				}
				else
				{

                    answerLabel.Content = string.Empty;
                    answerBtn.Content = "Show Answer";
                }
			}
        }

        void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            helper.SendPracticeProblemEmail(user, EmailTextBox.Text);
        }
    }
}
