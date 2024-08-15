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
using MultiCalculator.Helpers;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for PracticeProblems.xaml
	/// </summary>
	public partial class PracticeProblemsWindow : Window
	{
		public PracticeProblemsWindow()
		{
			InitializeComponent();
		}

		private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var user = new UserModel(); // this isn't done, idk how to get the currently logged-in user tho so someone gotta fix this up rq
			PracticeProblemsHelper.SendPracticeProblemEmail(user, (string)EmailTextBox.Text);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var practiceProblem = PracticeProblemsHelper.GeneratePracticeProblem();
			QuestionTextBlock.Text = practiceProblem;
		}
	}
}
