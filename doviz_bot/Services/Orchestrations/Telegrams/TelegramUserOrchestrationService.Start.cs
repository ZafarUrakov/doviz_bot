using System.Linq;
using System.Threading.Tasks;
using doviz_bot.Models.TelegramUserMessages;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService
    {
        private async ValueTask<bool> StartAsync(TelegramUserMessage telegramUserMessage)
        {
            if (telegramUserMessage.Message?.Text == startCommand)
            {
                var telegramUser = this.telegramUserService.RetriveAllTelegramUsers()
                    .FirstOrDefault(u => u.TelegramId == telegramUserMessage.Message.Chat.Id);

                if (string.IsNullOrWhiteSpace(telegramUser.PhoneNumber))
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
                else
                {
                    await BackToMenu(telegramUserMessage);

                    return true;
                }
            }
            else
            {
                var telegramUser = this.telegramUserService.RetriveAllTelegramUsers()
                   .FirstOrDefault(u => u.TelegramId == telegramUserMessage.Message.Chat.Id);

                if (string.IsNullOrWhiteSpace(telegramUser.PhoneNumber))
                {
                    await telegramService.SendMessageAsync(
                    userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                    message: $"Doviz 💸\n\nPlease press the \"/start\"");

                    return true;
                }
            }

            return false;
        }
    }
}
