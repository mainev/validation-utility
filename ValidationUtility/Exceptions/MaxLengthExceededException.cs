using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class MaxLengthExceededException : Exception
    {
        public MaxLengthExceededException(string value, string column, int maxLength)
            : base($"Value \"{value}\" reached maximum length of column:{column} maxLength={maxLength}, providedLength={value.Length}")
        {
        }
    }
}
