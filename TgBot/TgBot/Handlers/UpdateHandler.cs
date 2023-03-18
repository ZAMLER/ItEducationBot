

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
                    await CheckQuestion(botClient,update);
                    break;

            }
                
        }

        private async Task CheckQuestion(ITelegramBotClient botClient, Update update)
        {
            if(update.Message.Text == "Вчитись")
            {
                var questionInfo = await _questionService.GetQuestionInfo();
                await botClient.SendPollAsync(update.Message.Chat.Id, questionInfo.Question,questionInfo.Answers,false,PollType.Quiz,false,questionInfo.RightAnswer);
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
