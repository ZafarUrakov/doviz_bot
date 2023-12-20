using doviz_bot.Models.Converters;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace doviz_bot.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Converter> InsertConverterAsync(Converter converter);
        IQueryable<Converter> SelectAllConverters();
        ValueTask<Converter> SelectConverterByIdAsync(Guid converterId);
        ValueTask<Converter> UpdateConverterAsync(Converter converter);
        ValueTask<Converter> DeleteConverterAsync(Converter converter);
    }
}
