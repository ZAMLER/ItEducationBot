

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TgBot.Enums;
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
            Console.WriteLine(exception.Message);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch(update.Type)
            {
                case UpdateType.Message :
                   await CheckUserInDB(update);
                    await CheckCommands(botClient,update);
                    break;

            }
                
        }

        private async Task CheckCommands(ITelegramBotClient botClient, Update update)
        {
            switch (update.Message.Text)
            {
                case "Вчитись":
                    var questionInfo = await _questionService.GetQuestionInfo();
                    await botClient.SendPollAsync(update.Message.Chat.Id, questionInfo.Question, questionInfo.Answers, false, PollType.Quiz, false, questionInfo.RightAnswer);
                    break;
                case "Додати питання" :
                    await _userService.ChangeUserStateById(update.Message.From.Id, State.AddQuestion);
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Вводьте ваше питання");
                    break;
            }
            
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
