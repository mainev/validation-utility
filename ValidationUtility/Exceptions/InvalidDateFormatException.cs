using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class InvalidDateFormatException : Exception
    {
        public InvalidDateFormatException(string targetColumn, string dateFormat, string dateValue, string exceptionMessage) :
            base($"Date format \"{dateFormat}\" cannot be parsed for target column \"{targetColumn}\". Date Value : \"{dateValue}\". Exception Message: {exceptionMessage}")
        { 
        }
    }
}
