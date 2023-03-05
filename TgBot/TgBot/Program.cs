
using Telegram.Bot;
using TgBot;
using TgBot.Handlers;

var client = new TelegramBotClient(Constants.Token);
var updateHandler = new UpdateHandler();
var me = await client.GetMeAsync();
client.StartReceiving(updateHandler);
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
