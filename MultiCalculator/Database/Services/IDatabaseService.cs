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
        List<CalculationHistoryModel> LoadAllCalculationHistory();
        List<CalculationHistoryModel> LoadAllCalculationHistoryBasedOffUser(UserModel user);
        List<OpenAiQuestionsModel> LoadAllOpenAiQuestions();
        List<OpenAiQuestionsModel> LoadAllOpenAiQuestionsBasedOffUser(UserModel user);
        void AddUser(UserModel user);
        void AddCalculationHistory(CalculationHistoryModel calculationHistory);
        void AddOpenAiQuestion(OpenAiQuestionsModel openAiQuestion);
        void SeedData();
        void ClearData();
    }
}
