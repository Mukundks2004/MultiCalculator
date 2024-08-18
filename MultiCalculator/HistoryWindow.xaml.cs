
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using MultiCalculator.Helpers;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
	{
		readonly IDatabaseService _databaseService;
		readonly PdfWriterHelper _pdfWriterHelper;
		readonly UserModel _user;

		public HistoryWindow(IDatabaseService databaseService, UserModel user)
		{
			_databaseService = databaseService;
			_pdfWriterHelper = new PdfWriterHelper();
			_user = user;
			InitializeComponent();


			var history = _databaseService.LoadAllCalculationHistory();
			foreach (var questionAndAnswer in history)
			{
				var itm = new ListBoxItem()
				{
					Content = questionAndAnswer.Question + " = " + questionAndAnswer.Answer,
					Tag = (questionAndAnswer.Question, questionAndAnswer.Answer)
				};

				HistoryListBox.Items.Add(itm);
			}
		}

		void GeneratePdf_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var questionsAndAnswers = new List<(string, string)>();

				foreach (var selectedItem in HistoryListBox.SelectedItems)
				{
					if (selectedItem is ListBoxItem listBoxItem && listBoxItem.Tag is (string item1, string item2))
					{
						questionsAndAnswers.Add((item1, item2));
					}
				}

				var location = _pdfWriterHelper.GenerateQuestionSheet(questionsAndAnswers, _user);

				Process.Start(new ProcessStartInfo(location) { UseShellExecute = true });
			}
			catch (Exception ex)
			{
				var t = ex.Message;
				MessageBox.Show("Error generating pdf");
			}
		}
	}
}
