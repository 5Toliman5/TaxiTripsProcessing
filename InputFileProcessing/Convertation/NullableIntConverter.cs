using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputFileProcessing.Convertation
{
    public class NullableIntConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            if (string.IsNullOrEmpty(from))
                return null;
            if (int.TryParse(from, out int result))
                return result;

            throw new ConvertException(from, typeof(int));
        }
    }
}
