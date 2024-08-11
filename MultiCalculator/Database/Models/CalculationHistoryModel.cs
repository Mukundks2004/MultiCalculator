using System.ComponentModel.DataAnnotations;

namespace MultiCalculator.Database.Models
{
    public class CalculationHistoryModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string? AnswerSteps { get; set; }
        public UserModel QuestionSender { get; set; }
    }
}
