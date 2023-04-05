using Telegram.Bot;
using TgBot;
using TgBot.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var client = new TelegramBotClient(Constants.Token);
var updateHandler = new UpdateHandler();
var me = await client.GetMeAsync();
client.StartReceiving(updateHandler);
Console.WriteLine($"Start listening for @{me.Username}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
