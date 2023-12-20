using System.Threading.Tasks;
using doviz_bot.Models.TelegramUserMessages;
using doviz_bot.Services.Foundations.Converters;
using doviz_bot.Services.Foundations.Telegrams;
using doviz_bot.Services.Foundations.TelegramUsers;
using doviz_bot.Services.Processings.Telegrams;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService : ITelegramUserOrchestrationService
    {
        private readonly ITelegramUserProcessingService telegramUserProcessingService;
        private readonly ITelegramService telegramService;
        private readonly ITelegramUserService telegramUserService;
        private readonly IConverterService converterService;

        public TelegramUserOrchestrationService(
            ITelegramUserProcessingService telegramUserProcessingService,
            ITelegramService telegramService,
            ITelegramUserService telegramUserService,
            IConverterService converterService)
        {
            this.telegramUserProcessingService = telegramUserProcessingService;
            this.telegramService = telegramService;
            this.telegramUserService = telegramUserService;
            this.converterService = converterService;
        }

        private const string startCommand = "/start";
        private const string convertCommand = "💰 Convert";
        private const string menuCommand = "⬅️Menu";

        public async ValueTask<TelegramUserMessage> ProcessTelegramUserAsync(TelegramUserMessage telegramUserMessage)
        {
            telegramUserMessage.TelegramUser =
                await telegramUserProcessingService
                    .UpsertTelegramUserProcessingService(telegramUserMessage.TelegramUser);

            if (await BackToMenu(telegramUserMessage))
                return telegramUserMessage;

            if (await StartAsync(telegramUserMessage))
                return telegramUserMessage;

            if (await RegisterAsync(telegramUserMessage))
                return telegramUserMessage;

            if (await ConvertAsync(telegramUserMessage))
                return telegramUserMessage;

            return telegramUserMessage;
        }

        public void ListenTelegramUserMessage()
        {
            this.telegramService.RegisterTelegramEventHandler(async (telegramUserMessage) =>
            {
                await this.ProcessTelegramUserAsync(telegramUserMessage);
            });
        }

        private static ReplyKeyboardMarkup MainMarkupEng()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
                new KeyboardButton[]{new KeyboardButton("💰 Convert")},
                new KeyboardButton[]
                {
                    new KeyboardButton("✍️ Leave feedback"),
                    new KeyboardButton("ℹ️ Connect with us")
                },
            })
            {
                ResizeKeyboard = true
            };
        }
    }
}
