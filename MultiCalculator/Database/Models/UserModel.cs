using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiCalculator.Database.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [InverseProperty("QuestionSender")]
        public ICollection<CalculationHistoryModel> calculationHistory { get; set; } = new List<CalculationHistoryModel>();
        [InverseProperty("QuestionSender")]
        public ICollection<OpenAiQuestionsModel> openAiQuestions { get; set; } = new List<OpenAiQuestionsModel>();
    }
}
