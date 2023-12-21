using System;
using System.Linq;
using System.Threading.Tasks;
using doviz_bot.Models.Converters;

namespace doviz_bot.Services.Foundations.Converters
{
    public interface IConverterService
    {
        ValueTask<Converter> AddConverterAsync(Converter user);
        IQueryable<Converter> RetriveAllConverters();
        ValueTask<Converter> RetrieveConverterByIdAsync(Guid converterId);
        ValueTask<Converter> ModifyConverterAsync(Converter converter);
        ValueTask<Converter> RemoveConverterAsync(Converter converter);
    }
}
