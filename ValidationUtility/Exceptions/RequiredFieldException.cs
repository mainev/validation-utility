using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Exceptions
{
    public class RequiredFieldException : Exception
    {
        public RequiredFieldException(string target, string source)
            : base($"SourceColumn \"{source}\" violates REQUIRED property of TargetColumn \"{target}\".")
        {
        }
    }
}
