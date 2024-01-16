using System.Globalization;
using System.Text.RegularExpressions;
using ValidationUtility.Types;
using ValidationUtility.Exceptions;
using ValidationUtility.Models;

namespace ValidationUtility
{
    public static class DataValidator
    {
        /// <summary>
        /// Validates data based on dataField configuration.
        /// </summary>
        public static object? Validate(DataField dataField, object fieldValue)
        {
            object result = fieldValue;

            // Validates for required fields, throwing an exception if no value is provided
            if (dataField.Required && String.IsNullOrEmpty(Convert.ToString(fieldValue)))
                throw new RequiredFieldException(dataField.TargetColumn, dataField.SourceColumn);

            // Validates for non-required fields, returning null if no value is provided
            if (!dataField.Required && String.IsNullOrEmpty(Convert.ToString(fieldValue)))
                return null;

            if (fieldValue.GetType() == typeof(string))
            {
                string resultStr = Convert.ToString(result);

                switch (dataField.DataFormatType)
                {
                    case DataFormatType.Date:
                        result = ApplyDateValidation(dataField, resultStr);
                        break;
                    case DataFormatType.Regex:
                        result = ApplyRegexValidation(dataField, resultStr);
                        break;
                    case DataFormatType.Number:
                        result = ApplyNumberValidation(dataField, resultStr);
                        break;
                }

                // Check if the length of the result, after applying validations, exceeds the TargetColumnMaxLength.
                // If the length surpasses the maximum and the truncate option is enabled, truncate the result to TargetColumnMaxLength.
                // Otherwise, throw a MaxLengthReachedException.
                if (result != null)
                {
                    if ((Convert.ToString(result).Length > dataField.TargetColumnMaxLength) && dataField.TargetColumnMaxLength > 1)
                    {
                        if (dataField.TruncateMaxLengthExceededColumn)
                            result = Convert.ToString(result).Substring(0, (int)dataField.TargetColumnMaxLength);
                        else
                            throw new MaxLengthExceededException(Convert.ToString(result), dataField.TargetColumn, (int)dataField.TargetColumnMaxLength);
                    }
                }

            }

            return result;
        }

        public static string ApplyNumberValidation(DataField dataField, string value)
        {
            string newValue = value;

            try
            {
                // Try to convert the data to a number using the provided culture
                if (!String.IsNullOrEmpty(dataField.Culture))
                {
                    var inputCulture = new CultureInfo(dataField.Culture);
                    decimal.TryParse(value, NumberStyles.Any, inputCulture, out decimal decimalValue);
                    return decimalValue.ToString();
                }

                // Remove thousands separator to allow conversion to a valid number, if specified
                if (!String.IsNullOrEmpty(dataField.ThousandsSeparator))
                    newValue = newValue.Replace(dataField.ThousandsSeparator, "");

                // Remove decimal separator to allow conversion to a valid number, if specified
                if (!String.IsNullOrEmpty(dataField.DecimalSeparator))
                    newValue = newValue.Replace(dataField.DecimalSeparator, ".");

                bool canConvert = decimal.TryParse(newValue, out decimal outputNumber);
                if (!canConvert)
                {
                    switch (dataField.InvalidDataAction)
                    {
                        case DataFormatInvalidActionType.Empty:
                            return String.Empty;
                        case DataFormatInvalidActionType.Ignore:
                            return value;
                        case DataFormatInvalidActionType.Reject:
                            throw new InvalidNumberFormatException(dataField.TargetColumn, value, "Value not convertible to number.");
                    }
                }
            }
            catch (Exception)
            {
                switch (dataField.InvalidDataAction)
                {
                    case DataFormatInvalidActionType.Empty:
                        return String.Empty;
                    case DataFormatInvalidActionType.Ignore:
                        return value;
                    case DataFormatInvalidActionType.Reject:
                        throw;
                }
            }

            return newValue;
        }

        public static DateTime ApplyDateValidation(DataField dataField, string value)
        {
            if (String.IsNullOrEmpty(dataField.GeneralFormat))
                throw new NullDateFormatException(dataField.TargetColumn);

            try
            {
                return DateTime.ParseExact(value, dataField.GeneralFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new InvalidDateFormatException(dataField.TargetColumn, dataField.GeneralFormat, value, ex.Message);
            }
        }

        public static string ApplyEmailValidation(DataField dataField, string value)
        {
            Regex r = new Regex(RegexType.EMAIL);
            bool regexValid = r.IsMatch(value);

            if (!regexValid)
            {
                if (dataField.InvalidDataAction == DataFormatInvalidActionType.Empty)
                    return String.Empty;

                var newValue = dataField.InvalidDataAction switch
                {
                    DataFormatInvalidActionType.Empty => String.Empty,
                    DataFormatInvalidActionType.Ignore => value,
                    DataFormatInvalidActionType.Reject => throw new RegexMismatchException(value, dataField.GeneralFormat),
                    _ => throw new NotImplementedException()
                };

                return newValue;
            }

            return value;
        }

        public static string ApplyRegexValidation(DataField dataField, string value)
        {
            Regex r = new Regex(dataField.GeneralFormat);
            bool regexValid = r.IsMatch(value);

            if (!regexValid)
            {
                var newValue = dataField.InvalidDataAction switch
                {
                    DataFormatInvalidActionType.Empty => String.Empty,
                    DataFormatInvalidActionType.Ignore => value,
                    DataFormatInvalidActionType.Reject => throw new RegexMismatchException(value, dataField.GeneralFormat),
                    _ => throw new NotImplementedException()
                };

                return newValue;
            }

            return value;
        }

        public static string ApplyMaxLengthValidation(DataField dataField, string value)
        {
            if ((value.Length > dataField.TargetColumnMaxLength) && dataField.TargetColumnMaxLength > 1)
            {
                if (dataField.TruncateMaxLengthExceededColumn)
                    return value.Substring(0, (int)dataField.TargetColumnMaxLength);

                else
                    throw new MaxLengthExceededException(value, dataField.TargetColumn, (int)dataField.TargetColumnMaxLength);
            }

            return value;
        }
    }
}
