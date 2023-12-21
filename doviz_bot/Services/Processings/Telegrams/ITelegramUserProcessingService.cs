using doviz_bot.Models.TelegramUsers;
using System.Threading.Tasks;

namespace doviz_bot.Services.Processings.Telegrams
{
    public interface ITelegramUserProcessingService
    {
        ValueTask<TelegramUser> ModifyTelegramUserAsync(TelegramUser telegramUser);
        ValueTask<TelegramUser> UpsertTelegramUserProcessingService(TelegramUser telegramUser);
    }
}
