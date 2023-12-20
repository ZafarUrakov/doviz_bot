using doviz_bot.Models.TelegramUserMessages;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService
    {
        private async ValueTask<bool> StartAsync(TelegramUserMessage telegramUserMessage)
        {
            if (telegramUserMessage.Message?.Text == startCommand)
            {
                await telegramService.SendMessageAsync(
                userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                message: "Doviz 💸\n\nAssalamu Alaykum, my friend, I think you need to use me to find out the exchange rate. \nPress the \"📱 Phone number\" button to register.",
                replyMarkup: new ReplyKeyboardMarkup(new KeyboardButton[] { KeyboardButton.WithRequestContact("📱 Phone number") })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                });

                return true;
            }

            return false;
        }
    }
}
