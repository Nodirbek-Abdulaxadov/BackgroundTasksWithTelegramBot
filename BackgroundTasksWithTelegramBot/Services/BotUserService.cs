using BackgroundTasksWithTelegramBot.Data;
using BackgroundTasksWithTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace BackgroundTasksWithTelegramBot.Services;

public class BotUserService : IBotUserService
{
    private readonly AppDbContext _dbContext;

    public BotUserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(Chat user)
    {
        if (user == null)
        {
            Console.WriteLine("User was null");
        }
        var userIsExist = _dbContext.Users.Any(u => u.ChatId == user.Id);
        if (!userIsExist)
        {
            var model = (BotUser)user;
            _dbContext.Users.Add(model);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<BotUser>> GetBotUsersAsync()
        => await _dbContext.Users.ToListAsync();
}
