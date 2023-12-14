using BackgroundTasksWithTelegramBot.Models;
using Telegram.Bot.Types;

namespace BackgroundTasksWithTelegramBot.Services;

public interface IBotUserService
{
    Task<List<BotUser>> GetBotUsersAsync();
    Task AddUserAsync(Chat user);
}