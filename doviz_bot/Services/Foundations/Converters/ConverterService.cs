using System;
using System.Linq;
using System.Threading.Tasks;
using doviz_bot.Brokers.Storages;
using doviz_bot.Models.Converters;

namespace doviz_bot.Services.Foundations.Converters
{
    public class ConverterService : IConverterService
    {
        private readonly IStorageBroker storageBroker;

        public ConverterService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }
        public async ValueTask<Converter> AddConverterAsync(Converter user) =>
            await this.storageBroker.InsertConverterAsync(user);

        public IQueryable<Converter> RetriveAllConverters() =>
            this.storageBroker.SelectAllConverters();

        public ValueTask<Converter> RetrieveConverterByIdAsync(Guid converterId) =>
            this.storageBroker.SelectConverterByIdAsync(converterId);

        public ValueTask<Converter> ModifyConverterAsync(Converter converter) =>
            this.storageBroker.UpdateConverterAsync(converter);

        public async ValueTask<Converter> RemoveConverterAsync(Converter converter) =>
            await this.storageBroker.DeleteConverterAsync(converter);
    }
}
