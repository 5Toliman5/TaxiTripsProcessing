using FileHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputFileProcessing.Convertation
{
    public class DateTimeConverter : ConverterBase
    {
        private string[] DateFormats = { "dd/MM/yyyy hh:mm:ss tt", "MM/dd/yyyy hh:mm:ss tt" };

        public override object StringToField(string from)
        {
            DateTime result;
            if (!DateTime.TryParseExact(from, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                throw new ConvertException(from, typeof(DateTime));
            return TimeZoneInfo.ConvertTimeToUtc(result, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }
    }
}
