using doviz_bot.Models.TelegramUsers;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace doviz_bot.Services.Foundations.TelegramUsers
{
    public interface ITelegramUserService
    {
        ValueTask<TelegramUser> AddTelegramUserAsync(TelegramUser user);
        IQueryable<TelegramUser> RetriveAllTelegramUsers();
        ValueTask<TelegramUser> RetrieveTelegramUserByIdAsync(Guid telegramUserId);
        ValueTask<TelegramUser> ModifyTelegramUserAsync(TelegramUser telegramUser);
    }
}
