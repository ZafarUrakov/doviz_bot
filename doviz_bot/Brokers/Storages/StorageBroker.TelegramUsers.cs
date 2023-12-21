using System;
using System.Linq;
using System.Threading.Tasks;
using doviz_bot.Models.TelegramUsers;
using Microsoft.EntityFrameworkCore;

namespace doviz_bot.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<TelegramUser> TelegramUsers { get; set; }

        public async ValueTask<TelegramUser> InsertTelegramUserAsync(TelegramUser telegramUser) =>
            await InsertAsync(telegramUser);

        public IQueryable<TelegramUser> SelectAllTelegramUsers() =>
        SelectAll<TelegramUser>();

        public async ValueTask<TelegramUser> SelectTelegramUserByIdAsync(Guid telegramUserId) =>
        await SelectAsync<TelegramUser>(telegramUserId);

        public async ValueTask<TelegramUser> UpdateTelegramUserAsync(TelegramUser telegramUser) =>
        await UpdateAsync(telegramUser);

        public async ValueTask<TelegramUser> DeleteTelegramUserAsync(TelegramUser telegramUser) =>
            await DeleteAsync(telegramUser);
    }
}
