using MultiCalculator.Database;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using MultiCalculator.Helpers;
using System.Diagnostics;
using System.Windows;

namespace MultiCalculator
{
	/// <summary>
	/// Interaction logic for HistoryWindow.xaml
	/// </summary>
	public partial class HistoryWindow : Window
	{
		IDatabaseService _databaseService;
		PdfWriterHelper _pdfWriterHelper;
		UserModel _user;

		public HistoryWindow(IDatabaseService databaseService, UserModel user)
		{
			_databaseService = databaseService;
			_pdfWriterHelper = new PdfWriterHelper();
			_user = user;
			InitializeComponent();
		}

		void GeneratePdf_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				var history = _databaseService.LoadAllCalculationHistory();
				var questionsAndAnswers = history.Select(x => (x.Question, x.Answer)).ToList();
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
