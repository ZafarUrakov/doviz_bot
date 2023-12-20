using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System;
using doviz_bot.Models.TelegramUsers;

namespace doviz_bot.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<TelegramUser> InsertTelegramUserAsync(TelegramUser telegramUser);
        IQueryable<TelegramUser> SelectAllTelegramUsers();
        ValueTask<TelegramUser> SelectTelegramUserByIdAsync(Guid telegramUserId);
        ValueTask<TelegramUser> UpdateTelegramUserAsync(TelegramUser telegramUser);
        ValueTask<TelegramUser> DeleteTelegramUserAsync(TelegramUser telegramUser);
    }
}
