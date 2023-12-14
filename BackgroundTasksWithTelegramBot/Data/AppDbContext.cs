using BackgroundTasksWithTelegramBot.Models;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTasksWithTelegramBot.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<BotUser> Users { get; set; }
}