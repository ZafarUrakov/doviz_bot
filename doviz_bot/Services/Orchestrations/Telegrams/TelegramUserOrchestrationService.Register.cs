using System.Threading.Tasks;
using doviz_bot.Models.TelegramUserMessages;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService
    {
        private async ValueTask<bool> RegisterAsync(TelegramUserMessage telegramUserMessage)
        {
            if (string.IsNullOrWhiteSpace(telegramUserMessage.TelegramUser.PhoneNumber))
            {
                if(telegramUserMessage.Message.Type == MessageType.Contact &&
                    telegramUserMessage.TelegramUser.TelegramId == telegramUserMessage.Message.Contact.UserId)
                {
                    string phoneNumber = telegramUserMessage.Message.Contact.PhoneNumber;
                    telegramUserMessage.TelegramUser.PhoneNumber = phoneNumber;

                    await this.telegramUserProcessingService
                        .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                    var markup = MainMarkupEng();

                    await this.telegramService.SendMessageAsync(
                        userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                        message: $"Doviz 👻\n\nThank you for registering {telegramUserMessage.TelegramUser.FirstName}, use it for good!",
                        replyMarkup: markup);

                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
