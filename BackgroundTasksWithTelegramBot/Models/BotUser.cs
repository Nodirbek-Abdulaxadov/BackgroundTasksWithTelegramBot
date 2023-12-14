using Telegram.Bot.Types;

namespace BackgroundTasksWithTelegramBot.Models;

public class BotUser
{
    public int Id { get; set; }

    public long ChatId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public static implicit operator BotUser(Chat chat)
        => new()
        {
            ChatId = chat.Id,
            FirstName = chat.FirstName,
            LastName = chat.LastName,
            UserName = chat.Username
        };
}