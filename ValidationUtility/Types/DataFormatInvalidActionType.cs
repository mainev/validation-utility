using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationUtility.Types
{
    public enum DataFormatInvalidActionType
    {
        /// <summary>
        /// Use this type to return an empty string result for invalid data.
        /// </summary>
        Empty,

        /// <summary>
        /// Use this type to ignore invalid data and return the same value.
        /// </summary>
        Ignore,

        /// <summary>
        /// Use this type to throw exception for invalid data.
        /// </summary>
        Reject,
    }
}
