using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBot.Entities
{
    public class QuestionUser
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public long QuestionId { get; set; }
        public int Count { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
