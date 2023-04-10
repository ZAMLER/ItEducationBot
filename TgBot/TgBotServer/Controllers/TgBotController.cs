using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using TgBot;
using TgBot.Handlers;

namespace TgBotServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TgBotController : ControllerBase
    {
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        [HttpGet("start")]
        public async Task Start()
        {
            var client = new TelegramBotClient(Constants.Token);
            var updateHandler = new UpdateHandler();
            var me = await client.GetMeAsync();
            client.StartReceiving(updateHandler,cancellationToken : cancellationTokenSource.Token);
            Console.WriteLine($"Start listening for @{me.Username}");
        }
        [HttpGet("stop")]
        public void Stop() 
        {
            cancellationTokenSource.Cancel();
        }
    }
}
