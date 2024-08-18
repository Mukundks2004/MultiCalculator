using MultiCalculator.Database.Models;
using MultiCalculator.Database.Services;
using MultiCalculator.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ChatBotWindow.xaml
    /// </summary>
    public partial class ChatBotWindow : Window
    {
        readonly IDatabaseService _databaseService;
        readonly UserModel _user;
        readonly OpenAiHelper openAiHelper;
        private ChatBotHistoryModel ChatBotHistory = new ChatBotHistoryModel();
        private ObservableCollection<string> chatHistory = new ObservableCollection<string>();

        public ChatBotWindow(IDatabaseService databaseService, UserModel user)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;
            openAiHelper = new OpenAiHelper(databaseService);
            ChatBotHistory.ChatBotUser = _user;
        }

        void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            var prompt = MessageTextBox.Text;
            chatHistory.Add(prompt);
            var result = openAiHelper.SubmitAndGetApiResponse(prompt, _user);
            ChatBotHistory.QuestionHistory.Add(prompt);
            ChatBotHistory.AnswerHistory.Add(result);
            chatHistory.Add(result);

            MessageTextBox.Text = string.Empty;
        }
    }
}
