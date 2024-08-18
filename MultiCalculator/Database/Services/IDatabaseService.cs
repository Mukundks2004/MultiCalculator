using MultiCalculator.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Services
{
    public interface IDatabaseService
    {
        UserModel? LoadUserById(int id);
        CalculationHistoryModel LoadCalculationHistoryById(Guid id);
        OpenAiQuestionsModel LoadOpenAiQuestionsById(Guid id);
        ChatBotHistoryModel LoadChatBotHistoryById(Guid id);
        List<CalculationHistoryModel> LoadAllCalculationHistory();
        List<CalculationHistoryModel> LoadAllCalculationHistoryBasedOffUser(UserModel user);
        List<OpenAiQuestionsModel> LoadAllOpenAiQuestions();
        List<OpenAiQuestionsModel> LoadAllOpenAiQuestionsBasedOffUser(UserModel user);
        List<ChatBotHistoryModel> LoadAllChatBotHistory();
        List<ChatBotHistoryModel> LoadAllChatBotHistoryBasedOffUser(UserModel user);
        void AddUser(UserModel user);
        void AddCalculationHistory(CalculationHistoryModel calculationHistory);
        void AddOpenAiQuestion(OpenAiQuestionsModel openAiQuestion);
        void AddChatBotHistory(ChatBotHistoryModel chatBotHistory);
        void SeedData();
        void ClearData();
    }
}
