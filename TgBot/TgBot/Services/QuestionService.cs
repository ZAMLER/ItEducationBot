using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgBot.DTO;
using TgBot.Entities;

namespace TgBot.Services
{
    public class QuestionService
    {
        public async Task<QuestionInfo> GetQuestionInfo()
        {
            using (var context = new TgBotContext())
            {
                var random = new Random();
                var questionIds = await context.Questions.Select(item=>item.Id).ToListAsync();
                var index = random.Next(questionIds.Count);
                var questionId = questionIds[index];
                var question = await context.Questions.SingleAsync(item => item.Id == questionId);
                context.Entry(question).Collection(item => item.Answers).Load();
                var questionInfo = new QuestionInfo();
                questionInfo.Question = question.Name;
                questionInfo.Answers = question.Answers.Select(item => item.Name).ToList();
                questionInfo.RightAnswer = question.Answers.Select((item,index)=>new {item.Id,index}).Where(item=>item.Id == question.RightAnswer).Select(item => item.index).First();
                return questionInfo;
            }
        }
        public async Task<long> CreateQuestion(string name)
        {
            using(var context = new TgBotContext())
            {
                var question = new Question();
                question.Name = name;
                context.Questions.Add(question);
                await context.SaveChangesAsync();
                return question.Id;
            }
        }
    }
}
