using MultiCalculator.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Repositories
{
    public class ChatBotHistoryRepository : Repository<ChatBotHistoryModel>
    {
        public ChatBotHistoryRepository(CalculatorDbContext context) : base(context)
        {
        }

        public CalculatorDbContext CalculatorDbContext
        {
            get { return _context as CalculatorDbContext; }
        }

        public IEnumerable<ChatBotHistoryModel> GetAllChatBotHistory()
        {
            return CalculatorDbContext.ChatBotHistory;
        }

        public IEnumerable<ChatBotHistoryModel> GetAllChatBotHistoryBasedOffUser(UserModel chatBotUser)
        {
            return CalculatorDbContext.ChatBotHistory.Where(a => a.ChatBotUser == chatBotUser);
        }

        public ChatBotHistoryModel GetChatBotHistoryById(Guid id)
        {
            return CalculatorDbContext.ChatBotHistory.Where(a => a.Id == id).First();
        }

        public void UpdateChatBotHistory(ChatBotHistoryModel updatedChatBotHistory)
        {
            var chatBotHistoryHistory = GetChatBotHistoryById(updatedChatBotHistory.Id);
            if (chatBotHistoryHistory != null)
            {
                chatBotHistoryHistory = updatedChatBotHistory;
                Update(chatBotHistoryHistory);
                SaveChanges();
            }
        }

        public void DeleteChatBotHistory(Guid id)
        {
            ChatBotHistoryModel chatBotHistoryHistory = GetChatBotHistoryById(id);
            if (chatBotHistoryHistory != null)
            {
                Remove(chatBotHistoryHistory);
                SaveChanges();
            }
        }
    }
}
