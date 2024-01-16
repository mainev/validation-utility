using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class InvalidNumberFormatException : Exception
    {
        public InvalidNumberFormatException(string targetColumn, string numberValue, string exceptionMessage) :
            base($"Cannot convert number : \"{numberValue}\" to number for target column \"{targetColumn}\". Exception Message: {exceptionMessage}")
        {
        }
    }
}
