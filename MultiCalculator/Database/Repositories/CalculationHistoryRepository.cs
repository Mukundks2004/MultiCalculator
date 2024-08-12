using Microsoft.EntityFrameworkCore;
using MultiCalculator.Database.Models;

namespace MultiCalculator.Database.Repositories
{
    public class CalculationHistoryRepository : Repository<CalculationHistoryModel>
    {
        public CalculationHistoryRepository(CalculatorDbContext context) : base(context)
        {
        }

        public CalculatorDbContext CalculatorDbContext
        {
            get { return _context as CalculatorDbContext; }
        }

        public IEnumerable<CalculationHistoryModel> GetAllCalculationHistory() 
        {
            return CalculatorDbContext.CalculationHistory;
        }

        public IEnumerable<CalculationHistoryModel> GetAllCalculationHistoryBasedOffUser(UserModel questionSender)
        {
            return CalculatorDbContext.CalculationHistory.Where(a => a.QuestionSender == questionSender);
        }

        public CalculationHistoryModel GetCalculationById(Guid id) 
        {
            return CalculatorDbContext.CalculationHistory.Where(a => a.Id == id).First();
        }

        public void UpdateCalculationHistory(CalculationHistoryModel updatedCalculationHistory) 
        {
            var calculationHistory = GetCalculationById(updatedCalculationHistory.Id);
            if (calculationHistory != null)
            {
                calculationHistory = updatedCalculationHistory;
                Update(calculationHistory);
                SaveChanges();
            }
        }

        public void DeleteCalculationHistory(Guid id)
        {
            CalculationHistoryModel calculationHistory = GetCalculationById(id);
            if (calculationHistory != null)
            {
                Remove(calculationHistory);
                SaveChanges();
            }
        }
    }
}
