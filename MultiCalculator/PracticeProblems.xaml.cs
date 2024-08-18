﻿using System.Windows;
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
		PracticeProblemsHelper helper;

		public PracticeProblemsWindow(IDatabaseService databaseService, UserModel user)
		{
			InitializeComponent();
			Icon = new BitmapImage(new Uri("pack://application:,,,./icon.png"));
			helper = new PracticeProblemsHelper(databaseService);
			_databaseService = databaseService;
			_user = user;
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
				if (answerLabel.Content.ToString() == string.Empty)
				{
					answerLabel.Content = "Answer: " + answer;
					answerLabel.Visibility = Visibility.Visible;
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
			if (QuestionTextBlock.Text != string.Empty && QuestionTextBlock.Text != null && !QuestionTextBlock.Text.Equals("Here will be the generated question"))
			{
				helper.SendPracticeProblemEmail(_user, QuestionTextBlock.Text, EmailTextBox.Text == string.Empty ? null : EmailTextBox.Text);
				MessageBox.Show("Email has been sent!");
            }
			else
			{
				MessageBox.Show("Please request for a question prior to sending an email.");
			}
        }
    }
}
