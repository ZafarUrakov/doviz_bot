using System;
using System.Linq;
using System.Threading.Tasks;
using doviz_bot.Brokers.Storages;
using doviz_bot.Models.TelegramUsers;

namespace doviz_bot.Services.Foundations.TelegramUsers
{
    public class TelegramUserService : ITelegramUserService
    {
        private readonly IStorageBroker storageBroker;

        public TelegramUserService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }
        public async ValueTask<TelegramUser> AddTelegramUserAsync(TelegramUser user) =>
            await this.storageBroker.InsertTelegramUserAsync(user);

        public IQueryable<TelegramUser> RetriveAllTelegramUsers() =>
            this.storageBroker.SelectAllTelegramUsers();

        public ValueTask<TelegramUser> RetrieveTelegramUserByIdAsync(Guid telegramUserId) =>
            this.storageBroker.SelectTelegramUserByIdAsync(telegramUserId);

        public ValueTask<TelegramUser> ModifyTelegramUserAsync(TelegramUser telegramUser) =>
            this.storageBroker.UpdateTelegramUserAsync(telegramUser);
    }
}
