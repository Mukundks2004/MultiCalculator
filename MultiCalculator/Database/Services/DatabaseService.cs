using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MultiCalculator.Database.Models;
using MultiCalculator.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Services
{
    public class DatabaseService : IDatabaseService
    {
        readonly CalculatorDbContext _dbContext;
        readonly UserRepository _userRepository;
        readonly CalculationHistoryRepository _calculationHistoryRepository;
        readonly OpenAiQuestionsRepository _openAiQuestionsRepository;
        readonly ChatBotHistoryRepository _chatBotHistoryRepository;

        public DatabaseService(CalculatorDbContext dbContext, UserRepository userRepository, CalculationHistoryRepository calculationHistoryRepository, OpenAiQuestionsRepository openAiQuestionsRepository, ChatBotHistoryRepository chatBotHistoryRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _calculationHistoryRepository = calculationHistoryRepository;
            _openAiQuestionsRepository = openAiQuestionsRepository;
            _chatBotHistoryRepository = chatBotHistoryRepository;
        }

        public UserModel? LoadUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public CalculationHistoryModel LoadCalculationHistoryById(Guid id)
        {
            return _calculationHistoryRepository.GetCalculationById(id);
        }

        public OpenAiQuestionsModel LoadOpenAiQuestionsById(Guid id)
        {
            return _openAiQuestionsRepository.GetOpenAiQuestionHistoryById(id);
        }

        public ChatBotHistoryModel LoadChatBotHistoryById(Guid id)
        {
            return _chatBotHistoryRepository.GetChatBotHistoryById(id);
        }

        public List<CalculationHistoryModel> LoadAllCalculationHistory()
        {
            return _calculationHistoryRepository.GetAllCalculationHistory().ToList();
        }

        public List<CalculationHistoryModel> LoadAllCalculationHistoryBasedOffUser(UserModel user)
        {
            return _calculationHistoryRepository.GetAllCalculationHistoryBasedOffUser(user).ToList();
        }

        public List<OpenAiQuestionsModel> LoadAllOpenAiQuestions()
        {
            return _openAiQuestionsRepository.GetAllOpenAiQuestionHistory().ToList();
        }

        public List<OpenAiQuestionsModel> LoadAllOpenAiQuestionsBasedOffUser(UserModel user)
        {
            return _openAiQuestionsRepository.GetAllOpenAiQuestionHistoryBasedOffUser(user).ToList();
        }
        public List<ChatBotHistoryModel> LoadAllChatBotHistory()
        {
            return _chatBotHistoryRepository.GetAllChatBotHistory().ToList();
        }

        public List<ChatBotHistoryModel> LoadAllChatBotHistoryBasedOffUser(UserModel user)
        {
            return _chatBotHistoryRepository.GetAllChatBotHistoryBasedOffUser(user).ToList();
        }

        public void AddUser(UserModel user)
        {
            _userRepository.Add(user);
        }

        public void AddCalculationHistory(CalculationHistoryModel calculationHistory)
        {
            _calculationHistoryRepository.Add(calculationHistory);
        }

        public void AddOpenAiQuestion(OpenAiQuestionsModel openAiQuestion)
        {
            _openAiQuestionsRepository.Add(openAiQuestion);
        }

        public void AddChatBotHistory(ChatBotHistoryModel chatBotHistory)
        {
            _chatBotHistoryRepository.Add(chatBotHistory);
        }


        public void SeedData()
        {
            var users = new List<UserModel>() {
                new UserModel() { Id = 11111, Password = "11111", FirstName = "Anakin", LastName = "Skywalker", Username = "Darth Vader", Email = "Admin@gmail.com", Phone = "0444444444", AmountOfGeneratedPdfs = 0 },
                new UserModel() { Id = 22222, Password = "22222", FirstName = "Dennis", LastName = "Hsu", Username = "Dennis", Email = "Admin@gmail.com", Phone = "0444444444", AmountOfGeneratedPdfs = 0 },
                new UserModel() { Id = 33333, Password = "33333", FirstName = "Mukund", LastName = "Srinivasan", Username = "Mukund", Email = "Admin@gmail.com", Phone = "0444444444", AmountOfGeneratedPdfs = 0 },
                new UserModel() { Id = 44444, Password = "44444", FirstName = "Matt", LastName = "Lam", Username = "Matt", Email = "Admin@gmail.com", Phone = "0444444444", AmountOfGeneratedPdfs = 0 },
            };

            var randomQuestions = new List<CalculationHistoryModel>()
            {
                new CalculationHistoryModel() { Id = new Guid(), Question = "5 + 5", Answer = "10", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "34 + 17", Answer = "51", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "29 - 7", Answer = "22", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "8 * 6", Answer = "48", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "56 / 8", Answer = "7", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "12 + 45", Answer = "57", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "100 - 39", Answer = "61", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "9 * 7", Answer = "63", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "81 / 9", Answer = "9", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "23 + 34", Answer = "57", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "50 - 25", Answer = "25", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "7 * 11", Answer = "77", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "72 / 8", Answer = "9", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "15 + 22", Answer = "37", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "99 - 33", Answer = "66", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "5 * 9", Answer = "45", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "90 / 10", Answer = "9", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "37 + 44", Answer = "81", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "80 - 29", Answer = "51", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "6 * 12", Answer = "72", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "56 / 7", Answer = "8", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "27 + 18", Answer = "45", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "40 - 19", Answer = "21", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "8 * 8", Answer = "64", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "96 / 12", Answer = "8", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "14 + 31", Answer = "45", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "72 - 28", Answer = "44", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "4 * 15", Answer = "60", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "81 / 9", Answer = "9", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "50 + 22", Answer = "72", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "65 - 24", Answer = "41", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "3 * 19", Answer = "57", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "84 / 7", Answer = "12", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "30 + 45", Answer = "75", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "60 - 22", Answer = "38", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "7 * 9", Answer = "63", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "66 / 6", Answer = "11", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "41 + 18", Answer = "59", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "72 - 15", Answer = "57", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "5 * 13", Answer = "65", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "90 / 5", Answer = "18", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "19 + 24", Answer = "43", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "56 - 33", Answer = "23", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "8 * 14", Answer = "112", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "64 / 8", Answer = "8", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "21 + 36", Answer = "57", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "80 - 29", Answer = "51", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "6 * 10", Answer = "60", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "90 / 15", Answer = "6", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "24 + 55", Answer = "79", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "88 - 40", Answer = "48", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "9 * 7", Answer = "63", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "54 / 9", Answer = "6", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "14 + 26", Answer = "40", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "91 - 22", Answer = "69", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "7 * 11", Answer = "77", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "72 / 8", Answer = "9", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "45 + 32", Answer = "77", QuestionSender = users[0] },
                new CalculationHistoryModel() { Id = Guid.NewGuid(), Question = "64 - 21", Answer = "43", QuestionSender = users[0] },
            };

            var questionHistory = new List<List<string>>();
            var answerHistory = new List<List<string>>();
            for (int i = 0; i < randomQuestions.Count; i++)
            {
                if (i % 10 == 0)
                {
                    questionHistory.Add(new List<string>());
                    answerHistory.Add(new List<string>());
                }
                questionHistory[i / 10].Add(users[0].Username + ": " + randomQuestions[i].Question);
                answerHistory[i / 10].Add("MDM AI Chat Bot: " + randomQuestions[i].Answer);
            }

            var chatBotHistory = new List<ChatBotHistoryModel>()
            {
                new ChatBotHistoryModel() { Id = new Guid(), QuestionHistory = questionHistory[0], AnswerHistory = answerHistory[0], ChatBotUser = users[0], DisplayName = "Previous Chat from: 15-Aug-2024 3:46:28 PM" },
                new ChatBotHistoryModel() { Id = new Guid(), QuestionHistory = questionHistory[1], AnswerHistory = answerHistory[1], ChatBotUser = users[0], DisplayName = "Previous Chat from: 15-Aug-2024 6:21:59 PM"  },
                new ChatBotHistoryModel() { Id = new Guid(), QuestionHistory = questionHistory[2], AnswerHistory = answerHistory[2], ChatBotUser = users[0], DisplayName = "Previous Chat from: 16-Aug-2024 11:41:01 PM"  },
                new ChatBotHistoryModel() { Id = new Guid(), QuestionHistory = questionHistory[3], AnswerHistory = answerHistory[3], ChatBotUser = users[0], DisplayName = "Previous Chat from: 17-Aug-2024 1:30:29 PM"  },
                new ChatBotHistoryModel() { Id = new Guid(), QuestionHistory = questionHistory[4], AnswerHistory = answerHistory[4], ChatBotUser = users[0], DisplayName = "Previous Chat from: 18-Aug-2024 3:20:00 PM"  },
            };

            users.ForEach(u => AddUser(u));
            randomQuestions.ForEach(q => AddCalculationHistory(q));
            chatBotHistory.ForEach(c => AddChatBotHistory(c));
        }

        public void ClearData()
        {
            _userRepository.ClearDatabase();
            _calculationHistoryRepository.ClearDatabase();
            _openAiQuestionsRepository.ClearDatabase();
            _chatBotHistoryRepository.ClearDatabase();
        }

        public void UpdateChatBot(ChatBotHistoryModel chatBotHistory)
        {
            _chatBotHistoryRepository.UpdateChatBotHistory(chatBotHistory);
        }
    }
}
