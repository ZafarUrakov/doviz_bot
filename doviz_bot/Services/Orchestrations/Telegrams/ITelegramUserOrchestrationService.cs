using doviz_bot.Models.TelegramUserMessages;
using System.Threading.Tasks;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public interface ITelegramUserOrchestrationService
    {
        ValueTask<TelegramUserMessage> ProcessTelegramUserAsync(TelegramUserMessage telegramUserMessage);
        void ListenTelegramUserMessage();
    }
}
