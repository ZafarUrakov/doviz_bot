using System;
using System.Collections.Generic;
using doviz_bot.Models.Converters;

namespace doviz_bot.Models.TelegramUsers
{
    public class TelegramUser
    {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public TelegramUserStatus Status { get; set; }
        public Guid HelperId { get; set; }
        public ICollection<Converter> Converts { get; set; }
    }
}
