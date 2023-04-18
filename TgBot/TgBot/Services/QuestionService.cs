using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var random = new Random();
            long questionId = 0;
            using (var context = new TgBotContext())
            {
                var questionIds = await context.Questions.Select(item => item.Id).ToListAsync();
                var index = random.Next(questionIds.Count);  
                questionId = questionIds[index];
            }
            var questionInfo = await GetQuestionInfoById(questionId);
            var newIndex = random.Next(questionInfo.Answers.Count);
            var temp = questionInfo.Answers[questionInfo.RightAnswer];
            questionInfo.Answers[questionInfo.RightAnswer] = questionInfo.Answers[newIndex];
            questionInfo.Answers[newIndex] = temp;
            return questionInfo;
        }

        public async Task<QuestionInfo> GetQuestionInfoById(long questionId)
        {
            using (var context = new TgBotContext())
            {
                var question = await context.Questions.SingleAsync(item => item.Id == questionId);
                context.Entry(question).Collection(item => item.Answers).Load();
                var questionInfo = new QuestionInfo();
                questionInfo.Question = question.Name;
                questionInfo.Answers = question.Answers.OrderBy(item => item.Id).Select(item => item.Name.Substring(0, item.Name.Length > 100 ? 100 : item.Name.Length)).ToList();
                questionInfo.RightAnswer = question.Answers
                    .Select((item, index) => new { item.Id, index })
                    .Where(item => item.Id == question.RightAnswer)
                    .Select(item => item.index)
                    .FirstOrDefault();
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
        public async Task<long> CreateAnswer(long questionId, string name)
        {
            using (var context = new TgBotContext())
            {
                var answer = new Answer();
                answer.Name = name;
                answer.QuestionId = questionId;
                context.Answers.Add(answer);
                await context.SaveChangesAsync();
                return answer.Id;
            }
        }
        public async Task UpdateRightAnswerById(long questionId, long rightAnswer)
        {
            using(var context = new TgBotContext())
            {
               var question = await context.Questions.FirstOrDefaultAsync(item => item.Id == questionId);
                if (question == null)
                {
                    return;
                }
                question.RightAnswer = rightAnswer;
               await context.SaveChangesAsync();
            }
        }
        public async Task<List<Answer>> GetAnswersByQuestionId(long questionId)
        {
            using (var context = new TgBotContext())
            {
                return await context.Answers.OrderBy(item => item.Id).Where(item => item.QuestionId == questionId).ToListAsync();
            }
        }
    }
}
