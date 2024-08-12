using MultiCalculator.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Repositories
{
    public class OpenAiQuestionsRepository : Repository<OpenAiQuestionsModel>
    {
        public OpenAiQuestionsRepository(CalculatorDbContext context) : base(context)
        {
        }

        public CalculatorDbContext CalculatorDbContext
        {
            get { return _context as CalculatorDbContext; }
        }

        public IEnumerable<OpenAiQuestionsModel> GetAllOpenAiQuestionHistory()
        {
            return CalculatorDbContext.OpenAiQuestions;
        }

        public IEnumerable<OpenAiQuestionsModel> GetAllOpenAiQuestionHistoryBasedOffUser(UserModel questionSender)
        {
            return CalculatorDbContext.OpenAiQuestions.Where(a => a.QuestionSender == questionSender);
        }

        public OpenAiQuestionsModel GetOpenAiQuestionHistoryById(Guid id)
        {
            return CalculatorDbContext.OpenAiQuestions.Where(a => a.Id == id).First();
        }

        public void UpdateopenAiQuestionHistory(OpenAiQuestionsModel updatedOpenAiQuestionHistory)
        {
            var openAiQuestionHistory = GetOpenAiQuestionHistoryById(updatedOpenAiQuestionHistory.Id);
            if (openAiQuestionHistory != null)
            {
                openAiQuestionHistory = updatedOpenAiQuestionHistory;
                Update(openAiQuestionHistory);
                SaveChanges();
            }
        }

        public void DeleteopenAiQuestionHistory(Guid id)
        {
            OpenAiQuestionsModel openAiQuestionHistory = GetOpenAiQuestionHistoryById(id);
            if (openAiQuestionHistory != null)
            {
                Remove(openAiQuestionHistory);
                SaveChanges();
            }
        }
    }
}
