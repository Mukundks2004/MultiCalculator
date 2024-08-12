using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiCalculator.Database.Models
{
    public class OpenAiQuestionsModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public UserModel QuestionSender { get; set; }
    }
}
