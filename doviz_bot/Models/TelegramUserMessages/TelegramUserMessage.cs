using doviz_bot.Models.TelegramUsers;
using Telegram.Bot.Types;

namespace doviz_bot.Models.TelegramUserMessages
{
    public class TelegramUserMessage
    {
        public TelegramUser TelegramUser { get; set; }
        public Message Message{ get; set; }
    }
}
