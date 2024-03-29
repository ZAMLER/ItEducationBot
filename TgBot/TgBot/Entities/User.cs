﻿

namespace TgBot.Entities
{
    public class User
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? LanguageCode { get; set; }
        public int State { get; set; }
        public string? StateData { get; set; }
        public List<QuestionUser> QuestionUsers { get; set; } = new List<QuestionUser>();
    }
}
