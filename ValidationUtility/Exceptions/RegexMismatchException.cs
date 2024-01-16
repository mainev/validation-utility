using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class RegexMismatchException : Exception
    {
        public RegexMismatchException(string value, string regex)
           : base($"Value \"{value}\" does not match regex \"{regex}\"")
        {
        }
    }
}
