
using BackgroundTasksWithTelegramBot.Models;
using BackgroundTasksWithTelegramBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BackgroundTasksWithTelegramBot.BackgroundTasks;

public class ValyutaService : IHostedService
{
    private readonly HttpClient _client = new();
    private Timer _timer;
    private Timer _timer2;
    private List<Valyuta> _valyutas = new(); 
    private readonly IServiceProvider serviceProvider;
    TelegramBotClient botClient = new("6566749542:AAGbDcOEnTywiTjdoAJ3_yp41_dHtxw7Dz4");

    public ValyutaService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
   

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer2 = new(LoadValyutas, null, 0, 1000);
        _timer = new(SendMessage, null, 0, 500);

        return Task.CompletedTask;
    }

    private async void LoadValyutas(object state)
    {
        var now = TimeOnly.FromDateTime(DateTime.Now);
        var time = new TimeOnly(0, 0, 0);

        if (now.Hour == time.Hour && 
            now.Minute == time.Minute && 
            now.Second == time.Second)
        {
            _valyutas.Clear();
            Console.WriteLine("Ma'lumotlar o'chirildi!");
        }

        if (_valyutas.Count == 0)
        {
            var response = await _client.GetAsync("https://cbu.uz/uz/arkhiv-kursov-valyut/json/");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _valyutas = JsonConvert.DeserializeObject<List<Valyuta>>(content);
            }
            Console.WriteLine("Ma'lumotlar yangilandi!");
        }
    }

    private void SendMessage(object state)
    {
        if (_valyutas.Count != 0)
        {
            var date = DateTime.Now;
            Valyuta dollar = _valyutas[0];
            Valyuta yevro = _valyutas[1];
            Valyuta rubl = _valyutas[2];
            string kurs = $"""
            ----------------------------------------

            Sana: {date.Day}.{date.Month}.{date.Year}

            💸AQSh dollari: {dollar.Ccy} {dollar.Rate} so'm
            💶Yevro: {yevro.Ccy} {yevro.Rate} so'm
            🤑Rubl: {rubl.Ccy} {rubl.Rate} so'm

            ----------------------------------------
            """;
            using CancellationTokenSource cts = new();
            
            SendMessageToAllUsers(kurs, cts.Token);
        }
    }

    private async void SendMessageToAllUsers(string message, CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var botUserService = scope.ServiceProvider.GetService<IBotUserService>();
        var users = await botUserService.GetBotUsersAsync();

        foreach (var user in users)
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: user.ChatId,
            text: message,
            cancellationToken: cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}