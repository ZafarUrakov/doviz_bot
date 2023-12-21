using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using doviz_bot.Models.Converters;
using doviz_bot.Models.TelegramUserMessages;
using doviz_bot.Models.TelegramUsers;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace doviz_bot.Services.Orchestrations.Telegrams
{
    public partial class TelegramUserOrchestrationService
    {
        private async ValueTask<bool> ConvertAsync(TelegramUserMessage telegramUserMessage)
        {
            if (telegramUserMessage.Message.Text == convertCommand)
            {
                telegramUserMessage.TelegramUser.Status = TelegramUserStatus.Convert;
                await this.telegramUserProcessingService
                        .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                var markup = CurrenciesMarkupEng();

                await this.telegramService.SendMessageAsync(
                    userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                    message: "Doviz 🚩\n\nEnter the code of the currency you are converting from (for example: RUB🇷🇺):",
                    replyMarkup: markup);

                return true;
            }
            else if (telegramUserMessage.TelegramUser.Status == TelegramUserStatus.Convert)
            {
                var currenciesMarkup = CurrenciesMarkupEng();

                foreach (var row in currenciesMarkup.Keyboard)
                {
                    foreach (var button in row)
                    {
                        if (telegramUserMessage.Message.Text == button.Text)
                        {
                            var converter = new Converter
                            {
                                Id = Guid.NewGuid(),
                                FirstCurrency = telegramUserMessage.Message.Text,
                                TelegramUserId = telegramUserMessage.TelegramUser.Id
                            };

                            await this.converterService.AddConverterAsync(converter);

                            telegramUserMessage.TelegramUser.Status = TelegramUserStatus.First;
                            telegramUserMessage.TelegramUser.HelperId = converter.Id;
                            await this.telegramUserProcessingService
                                   .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                            var markup = new ReplyKeyboardMarkup(new KeyboardButton[] { "⬅️Menu" })
                            {
                                ResizeKeyboard = true
                            };

                            await this.telegramService.SendMessageAsync(
                                userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                                message: $"Doviz 💵\n\nEnter now the {telegramUserMessage.Message.Text} currency amount",
                                replyMarkup: markup);

                            return true;
                        }
                    }
                }
            }
            else if (telegramUserMessage.TelegramUser.Status == TelegramUserStatus.First)
            {
                var converter = this.converterService.RetriveAllConverters()
                    .FirstOrDefault(c => c.Id == telegramUserMessage.TelegramUser.HelperId);

                if (converter != null)
                {
                    string amountString = Regex.Replace(telegramUserMessage.Message.Text, "[^0-9]", "");

                    if (float.TryParse(amountString, out float convertedAmount))
                    {
                        converter.Amount = (decimal)convertedAmount;
                        await this.converterService.ModifyConverterAsync(converter);

                        telegramUserMessage.TelegramUser.Status = TelegramUserStatus.Amount;
                        await this.telegramUserProcessingService
                            .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                        var markup = CurrenciesMarkupEng();

                        await this.telegramService.SendMessageAsync(
                            userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                            message: $"Doviz 💵\n\n Select the currency into which you want to convert {converter.FirstCurrency}",
                            replyMarkup: markup);
                    }


                    return false;
                }

                return false;
            }
            else if (telegramUserMessage.TelegramUser.Status == TelegramUserStatus.Amount)
            {
                var currenciesMarkup = CurrenciesMarkupEng();

                foreach (var row in currenciesMarkup.Keyboard)
                {
                    foreach (var button in row)
                    {
                        if (telegramUserMessage.Message.Text == button.Text)
                        {
                            var converter = this.converterService.RetriveAllConverters()
                                .FirstOrDefault(c => c.Id == telegramUserMessage.TelegramUser.HelperId);

                            if (converter != null)
                            {
                                converter.SecondCurrency = telegramUserMessage.Message.Text;
                                await this.converterService.ModifyConverterAsync(converter);

                                string firstCurrency = converter.FirstCurrency;
                                string firstCurrencyCode = firstCurrency.Substring(0, 3);

                                string secondCurrency = converter.SecondCurrency;
                                string secondCurrencyCode = secondCurrency.Substring(0, 3);

                                telegramUserMessage.TelegramUser.Status = TelegramUserStatus.Last;
                                await this.telegramUserProcessingService
                                    .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                                var waitMessage = await this.telegramService.SendMessageAsync(
                                    userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                                    message: $"Doviz 🔄\n\n Wait...");

                                string apiKey = "4362d7427e92bf6088063ea3";
                                string apiUrl = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{firstCurrencyCode}";

                                using (WebClient webClient = new WebClient())
                                {
                                    string json = webClient.DownloadString(apiUrl);

                                    JObject jsonData = JObject.Parse(json);

                                    decimal exchangeRate = (decimal)jsonData["conversion_rates"][secondCurrencyCode];

                                    int result = (int)(converter.Amount * exchangeRate);

                                    var markup = MainMarkupEng();

                                    await this.telegramService.SendMessageAsync(
                                        userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                                        message: $"Doviz 🫡\n\n{converter.Amount} {firstCurrency} = {result} {secondCurrency}",
                                        replyMarkup: markup);

                                    converter.Result = result;

                                    await this.converterService.ModifyConverterAsync(converter);

                                    await this.telegramService.DeleteMessageAsync(
                                        userTelegramId: telegramUserMessage.TelegramUser.TelegramId,
                                        messageId: waitMessage.MessageId);

                                    telegramUserMessage.TelegramUser.Status = TelegramUserStatus.Active;
                                    await this.telegramUserProcessingService
                                        .ModifyTelegramUserAsync(telegramUserMessage.TelegramUser);

                                    return true;
                                }
                            }

                            return true;
                        }
                    }
                }
            }
            else if (telegramUserMessage.TelegramUser.Status == TelegramUserStatus.Amount)
            {

            }

            return false;
        }

        private static ReplyKeyboardMarkup CurrenciesMarkupEng()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("USD🇺🇸"),
                    new KeyboardButton("RUB🇷🇺"),
                    new KeyboardButton("UZS🇺🇿")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("EUR🇪🇺"),
                    new KeyboardButton("KRW🇰🇷"),
                    new KeyboardButton("CNY🇨🇳")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("TRY🇹🇷"),
                    new KeyboardButton("KZT🇰🇿"),
                    new KeyboardButton("KGS🇰🇮")
                },
               new KeyboardButton[]
                {
                    new KeyboardButton("⬅️Menu"),
                },
            })
            {
                ResizeKeyboard = true
            };
        }
    }
}
