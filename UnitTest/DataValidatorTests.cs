using ValidationUtility;
using ValidationUtility.Exceptions;
using ValidationUtility.Models;
using ValidationUtility.Types;

namespace UnitTest
{
    internal class DataValidatorTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void DateValidation_Test()
        {
            var dataField = new DataField()
            {
                DataFormatType = DataFormatType.Date,
                InvalidDataAction = DataFormatInvalidActionType.Reject,
                GeneralFormat = "yyyy-MM-dd"
            };

            string inputValue = "20240101";

            // Test date format validation
            // Should throw an exception since the input is not in the specified date format
            Assert.Throws<InvalidDateFormatException>(() => DataValidator.Validate(dataField, inputValue));
        }

        [Test]
        public void DecimalValidationWithCulture_Test()
        {
            var dataField = new DataField()
            {
                DataFormatType = DataFormatType.Number,
                InvalidDataAction = DataFormatInvalidActionType.Reject
            };

            string inputValue = "1.65";

            // Test for US culture, string should be considered as a valid number
            dataField.Culture = "en-US";
            string formattedNumber = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual(formattedNumber, "1.65");

            // Test for French Culture, string should be considered as a valid number
            dataField.Culture = "fr-FR";
            inputValue = "1,650";
            formattedNumber = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual(formattedNumber, "1.650");
        }

        [Test]
        public void DecimalValidationWithSeparator_Test()
        {
            var dataField = new DataField()
            {
                DataFormatType = DataFormatType.Number,
                InvalidDataAction = DataFormatInvalidActionType.Reject,
                ThousandsSeparator = "",
                DecimalSeparator = "."
            };

            string inputValue = "0.0";

            // Test if the string contains a period decimal separator, string should be considered as a valid number
            string formattedNumber = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual(formattedNumber, "0.0");
        }

        [Test]
        public void DecimalValidationWithSeparator_TestRejectException()
        {
            var dataField = new DataField()
            {
                DataFormatType = DataFormatType.Number,
                InvalidDataAction = DataFormatInvalidActionType.Reject,
                ThousandsSeparator = "",
                DecimalSeparator = ","
            };

            string inputValue = "1,65AWERTY";

            // Test if the input contains a comma decimal separator, string should not be considered as a valid number
            // If not a valid number, the validator should throw an exception since the InvalidDataAction is set to Reject
            Assert.Throws<InvalidNumberFormatException>(() => DataValidator.Validate(dataField, inputValue));

        }

        [Test]
        public void DecimalValidationWithSeparator_TestIgnoreException()
        {
            var dataField = new DataField()
            {
                DataFormatType = DataFormatType.Number,
                InvalidDataAction = DataFormatInvalidActionType.Ignore,
                ThousandsSeparator = "",
                DecimalSeparator = ","
            };

            string inputValue = "1,65AWERTY";

            // Test if the input contains a comma decimal separator, string should not be considered as a valid number
            // If not a valid number, the validator should return the same value since the InvalidDataAction is set to Ignore
            Assert.AreEqual(inputValue, (string)DataValidator.Validate(dataField, inputValue));
        }

        [Test]
        public void Truncate_Test()
        {
            var dataField = new DataField()
            {
                TargetColumnMaxLength = 6,
                TruncateMaxLengthExceededColumn = true
            };

            string inputValue = "Albert Einstein";

            // Test if the input length is within the TargetMaxColumnLength
            // If the length exceeds the TargetMaxColumnLength, then the input should be truncated
            string truncatedInput = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual("Albert", truncatedInput);
        }

        [Test]
        public void RegexValidation_TestValidRegex()
        {

            var dataField = new DataField
            {
                InvalidDataAction = DataFormatInvalidActionType.Empty,
                DataFormatType = DataFormatType.Regex,
                GeneralFormat = @"\$\{.*?\}"
            };

            var inputValue = "${TTEESTTT}";

            // Test if the input matches the specified Regex format
            // Should return the same input value since it matches the specified regex format
            string outputvalue = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual(outputvalue, inputValue);
        }

        [Test]
        public void RegexValidation_TestEmptyRegex()
        {
            var dataField = new DataField
            {
                InvalidDataAction = DataFormatInvalidActionType.Empty,
                DataFormatType = DataFormatType.Regex,
                GeneralFormat = @"\$\{.*?\}"
            };

            var inputValue = "${TTEES";

            // Test if the input matches the specified Regex format
            // Should return an empty string since the provided input does not match the regex format
            string outputvalue = (string)DataValidator.Validate(dataField, inputValue);
            Assert.AreEqual(String.Empty, outputvalue);
        }

        [Test]
        public void RegexValidation_TestRejectRegex()
        {
            var dataField = new DataField
            {
                InvalidDataAction = DataFormatInvalidActionType.Reject,
                DataFormatType = DataFormatType.Regex,
                GeneralFormat = @"\$\{.*?\}"
            };

            var inputValue = "${TTEES";

            // Test if the input matches the specified Regex format
            // Should throw an exception since the provided input does not match the regex format
            Assert.Throws<RegexMismatchException>(() => DataValidator.Validate(dataField, inputValue));
        }

    }
}
