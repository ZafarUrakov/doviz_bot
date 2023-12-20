using System;
using doviz_bot.Models.TelegramUsers;

namespace doviz_bot.Models.Converters
{
    public class Converter
    {
        public Guid Id { get; set; }
        public string FirstCurrency { get; set; }
        public decimal Amount { get; set; }
        public string SecondCurrency { get; set; }
        public decimal Result { get; set; }

        public Guid TelegramUserId { get; set; }
        public TelegramUser TelegramUser { get; set; }
    }
}
