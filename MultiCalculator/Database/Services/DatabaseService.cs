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

        public DatabaseService(CalculatorDbContext dbContext, UserRepository userRepository, CalculationHistoryRepository calculationHistoryRepository, OpenAiQuestionsRepository openAiQuestionsRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _calculationHistoryRepository = calculationHistoryRepository;
            _openAiQuestionsRepository = openAiQuestionsRepository;
        }

        public UserModel LoadUserById(int id)
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
    }
}
