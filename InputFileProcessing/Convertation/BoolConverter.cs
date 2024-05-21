using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputFileProcessing.Convertation
{
    public class BoolConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return from == "Y";

        }
    }
}
