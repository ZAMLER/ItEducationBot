

using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.Entities;
using TgBot.Enums;
using TgBot.Helpers;
using TgBot.Services;
using MyUser = TgBot.Entities.User;

namespace TgBot.Handlers
{
    public class UpdateHandler : IUpdateHandler
    {
        private QuestionService _questionService;
        private UserService _userService;
        public UpdateHandler() 
        {
            _userService = new UserService();
            _questionService = new QuestionService();
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            LoggerHelper.WriteLine(exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            LoggerHelper.WriteLine($"Update: {update.Type}");
            switch(update.Type)
            {
                case UpdateType.Message :
                   await CheckUserInDB(update);
                    await CheckStates(botClient, update);
                    break;

            }
                
        }
        public async Task CheckStates(ITelegramBotClient botClient, Update update)
        {
            var state = await _userService.GetUserStateById(update.Message.From.Id);
            LoggerHelper.WriteLine($"User : {update.Message.From.FirstName}; State: {state}");
            switch (state)
            {
                case State.AddQuestion:
                    await AddQuestionState(botClient, update);
                    break;
                case State.AddAnswer:
                    await AddAnswerState(botClient, update);
                    break;
                case State.ChooseRightAnswer:
                    await ChooseRightAnswerState(botClient, update);
                    break;
                default : 
                    await CheckCommands(botClient, update);
                    break;
            }
        }
        private async Task CheckCommands(ITelegramBotClient botClient, Update update)
        {
            LoggerHelper.WriteLine($"Message: {update.Message.Text}");
            switch (update.Message.Text)
            {
                case "Вчитись":
                    await LearnCommand(botClient, update);
                    break;
                case "Додати питання":
                    await AddQuestionCommand(botClient, update);
                    break;
            }

        }

        private async Task ChooseRightAnswerState(ITelegramBotClient botClient, Update update)
        {
            var qId = await _userService.GetUserStateDataById(update.Message.From.Id);
            var parsedQId = long.Parse(qId);
            var answers = await _questionService.GetAnswersByQuestionId(parsedQId);
            if (int.TryParse(update.Message.Text, out var index))
            {
                var answerr = answers[index - 1];
                await _questionService.UpdateRightAnswerById(parsedQId, answerr.Id);

                await _userService.ChangeUserStateById(update.Message.From.Id, State.None);
                await _userService.ChangeUserStateDataById(update.Message.From.Id, null);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Ви обрали правильну відповідь : {answerr.Name}");
            }
        }

        private async Task AddAnswerState(ITelegramBotClient botClient, Update update)
        {
            if (update.Message.Text == "Вихід")
            {
                await ExitCommand(botClient, update);
            }
            else
            {
                var question = await _userService.GetUserStateDataById(update.Message.From.Id);
                await _questionService.CreateAnswer(int.Parse(question), update.Message.Text);
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введіть відповіді або вихід");
            }
        }

        private async Task ExitCommand(ITelegramBotClient botClient, Update update)
        {
            var builder = new StringBuilder();
            await _userService.ChangeUserStateById(update.Message.From.Id, State.ChooseRightAnswer);
            builder.AppendLine("Оберіть правильну відповідь");
            var question = await _userService.GetUserStateDataById(update.Message.From.Id);
            var questionInfo = await _questionService.GetQuestionInfoById(long.Parse(question));
            for (int i = 0; i < questionInfo.Answers.Count; i++)
            {
                string? answer = questionInfo.Answers[i];
                builder.AppendLine($"{i + 1} : {answer}");
            }
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, builder.ToString());
        }

        private async Task AddQuestionState(ITelegramBotClient botClient, Update update)
        {
            var questionId = await _questionService.CreateQuestion(update.Message.Text);
            await _userService.ChangeUserStateById(update.Message.From.Id, State.AddAnswer);
            await _userService.ChangeUserStateDataById(update.Message.From.Id, questionId.ToString());
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Введіть відповіді або вихід");
        }

        private async Task AddQuestionCommand(ITelegramBotClient botClient, Update update)
        {
            await _userService.ChangeUserStateById(update.Message.From.Id, State.AddQuestion);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Вводьте ваше питання");
        }

        private async Task LearnCommand(ITelegramBotClient botClient, Update update)
        {
            var questionInfo = await _questionService.GetQuestionInfo();
            await botClient.SendPollAsync(update.Message.Chat.Id, questionInfo.Question, questionInfo.Answers, false, PollType.Quiz, false, questionInfo.RightAnswer);
        }

        private async Task CheckUserInDB(Update update)
        {
            if (!await _userService.CheckUserInDB(update.Message.From.Id))
            {
                var user = new MyUser();
                user.Id = update.Message.From.Id; 
                user.UserName = update.Message.From.Username;
                user.FirstName = update.Message.Chat.FirstName;
                user.LastName = update.Message.Chat.LastName;
                user.ChatId = update.Message.Chat.Id;
                await _userService.CreateUser(user);
            }
        }
    }
}
