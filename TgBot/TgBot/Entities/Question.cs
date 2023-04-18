using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Entities
{
    public class Question
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? RightAnswer { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
        public List<QuestionUser> QuestionUsers { get; set; } = new List<QuestionUser>();
    }
}
