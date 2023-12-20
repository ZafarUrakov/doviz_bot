using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Brokers.Telegrams
{
    public class TelegramBroker : ITelegramBroker
    {
        private readonly ITelegramBotClient telegramBotClient;
        private static Func<Update, ValueTask> taskHandler;

        public TelegramBroker(IConfiguration configuration)
        {
            string token = configuration["BotConfiguration:BotToken"];

            this.telegramBotClient = new TelegramBotClient(token);
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            this.telegramBotClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions);
        }
        private async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken ct) =>
            await taskHandler(update);

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public void RegisterTelegramEventHandler(Func<Update, ValueTask> eventHandler) =>
            taskHandler = eventHandler;

        public async ValueTask<Message> SendTextMessageAsync(
            long userTelegramId,
            string message,
            int? replyToMessageId = null,
            ParseMode? parseMode = null,
            IReplyMarkup? replyMarkup = null)
        {
            return await telegramBotClient.SendTextMessageAsync(
                chatId: userTelegramId,
                text: message,
                parseMode: parseMode,
                replyToMessageId: replyToMessageId,
                replyMarkup: replyMarkup);
        }

        public async ValueTask DeleteMessageAsync(
            long userTelegramId,
            int messageId)
        {
            await telegramBotClient.DeleteMessageAsync(
                chatId: userTelegramId,
                messageId: messageId);
        }
    }
}
