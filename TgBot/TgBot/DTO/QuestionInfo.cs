using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.DTO
{
    public class QuestionInfo
    {
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public int RightAnswer { get; set; }
    }
}
