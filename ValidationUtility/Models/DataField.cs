using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationUtility.Types;

namespace ValidationUtility.Models
{
    public class DataField
    {
        // Column Information
        public string SourceColumn { get; set; }
        public string TargetColumn { get; set; }
        public int? TargetColumnMaxLength { get; set; }
        public bool TruncateMaxLengthExceededColumn { get; set; }

        // Validation and Formatting
        public bool Required { get; set; }
        public DataFormatType DataFormatType { get; set; }
        public string GeneralFormat { get; set; }

        // Data Format Handling
        public DataFormatInvalidActionType InvalidDataAction { get; set; }

        // Culture and Formatting
        public string Culture { get; set; }
        public string ThousandsSeparator { get; set; }
        public string DecimalSeparator { get; set; }
    }
}
