using System.ComponentModel.DataAnnotations;

namespace MultiCalculator.Database.Models
{
    public class ChatBotHistoryModel
    {
        [Key]
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public List<string> QuestionHistory { get; set; } = new List<string>();
        public List<string> AnswerHistory { get; set; } = new List<string>();
        public UserModel ChatBotUser { get; set; }
    }
}
