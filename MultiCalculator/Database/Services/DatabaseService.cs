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

        public DatabaseService(CalculatorDbContext dbContext, UserRepository userRepository, CalculationHistoryRepository calculationHistoryRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _calculationHistoryRepository = calculationHistoryRepository;
        }

        public UserModel LoadUserById(int id)
        {
            return _userRepository.GetUserById(id);    
        }

        public CalculationHistoryModel LoadCalculationHistoryById(Guid id)
        {
            return _calculationHistoryRepository.GetCalculationById(id);
        }

        public List<CalculationHistoryModel> LoadAllCalculationHistory()
        {
            return _calculationHistoryRepository.GetAllCalculationHistory().ToList();
        }

        public List<CalculationHistoryModel> LoadAllCalculationHistoryBasedOffUser(UserModel user)
        {
            return _calculationHistoryRepository.GetAllCalculationHistoryBasedOffUser(user).ToList();
        }

        public void AddUser(UserModel user)
        {
            _userRepository.Add(user);
        }

        public void AddCalculationHistory(CalculationHistoryModel calculationHistory)
        {
            _calculationHistoryRepository.Add(calculationHistory);
        }

    }
}
