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
        private ChatBotHistoryModel ChatBotHistory;
        private List<ChatBotHistoryModel> chatBotHistoryModel;

        private ObservableCollection<string> messages;

        public ObservableCollection<string> Messages
        { 
            get { return messages; }
            set { messages = value; }
        }

        private ObservableCollection<string> previousChats;

        public ObservableCollection<string> PreviousChats
        {
            get { return previousChats; }
            set { previousChats = value; }
        }

        public ChatBotWindow(IDatabaseService databaseService, UserModel user)
        {
            DataContext = this;
            messages = new ObservableCollection<string>();
            previousChats = new ObservableCollection<string>();
            InitializeComponent();
            _databaseService = databaseService;
            _user = user;
            openAiHelper = new OpenAiHelper(databaseService);
            usernameLabel.Content = user.Username;
            fullNameLabel.Content = $"{user.FirstName} {user.LastName}";
            ChatBotHistory = new ChatBotHistoryModel();
            SetupPreviousChatsAndChats();
        }

        void SetupPreviousChatsAndChats()
        {
            chatBotHistoryModel = _databaseService.LoadAllChatBotHistoryBasedOffUser(_user);
            ChatBotHistory.ChatBotUser = _user;
            ChatBotHistory.DisplayName = "Previous Chat from: " + DateTime.Now.ToString();
            previousChats.Add("New chat");
            for (var i = chatBotHistoryModel.Count - 1; i >= 0; i--)
            {
                previousChats.Add(chatBotHistoryModel[i].DisplayName);
            }
            _databaseService.AddChatBotHistory(ChatBotHistory);
        }

        async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Equals(string.Empty))
            {
                MessageBox.Show("Please enter a prompt.");
            }
            else
            {
                var prompt = MessageTextBox.Text;
                messages.Add(_user.Username + ": " + prompt);
                var result = await openAiHelper.SubmitAndGetApiResponse(prompt, _user);
                ChatBotHistory.QuestionHistory.Add(prompt);
                ChatBotHistory.AnswerHistory.Add(result);
                messages.Add("MDM AI Chat Bot: " + result);
                _databaseService.UpdateChatBot(ChatBotHistory);
                MessageTextBox.Text = string.Empty;
            }
        }

        void GoToSelectedChat_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (string)PreviousChatslv.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }
            if (selectedItem.Equals("New chat"))
            {
                SetupSelectedChat(ChatBotHistory);
            }
            else
            {
                var selectedChat = chatBotHistoryModel.Where(a => a.DisplayName == selectedItem).First();
                SetupSelectedChat(selectedChat);
            }
        }

        void SetupSelectedChat(ChatBotHistoryModel chat)
        {
            messages.Clear();
            for (var i = 0; i < chat.QuestionHistory.Count; i++)
            {
                messages.Add(_user.Username + ": " + chat.QuestionHistory[i]);
                messages.Add("MDM AI Chat Bot: " + chat.AnswerHistory[i]);
            }
        }
    }
}
