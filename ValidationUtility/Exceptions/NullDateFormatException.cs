using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class NullDateFormatException : Exception
    {
        public NullDateFormatException(string targetColumn) :
            base($"No date format specified for targetColumn \"{targetColumn}\". Check ColumnMapper to change the format.")
        { 
        }
    }
}
