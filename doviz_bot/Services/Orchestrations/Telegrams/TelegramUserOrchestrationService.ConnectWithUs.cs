using doviz_bot.Models.TelegramUserMessages;
using System.Threading.Tasks;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService
    {
        private async ValueTask<bool> ConnectWithUsAsync(TelegramUserMessage telegramUserMessage)
        {
            if (telegramUserMessage.Message.Text == connectWithUseCommand)
            {
                await this.telegramService.SendMessageAsync(
                    userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                    message: "Doviz ⚡️\n\nHere are our contacts my dear friend: @zafar_urakov | @Johnnysenior");

                return true;
            }

            return false;
        }
    }
}
