namespace BookingAMaintenanceService.Dialogs.Utilities
{
    using Microsoft.Recognizers.Text;
    using Microsoft.Recognizers.Text.DateTime;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class DialogUtils
    {
        public static bool IsUserInputInOptions(string userInput, HashSet<string> options)
        {
            return options.Contains(userInput) || options.Any(option => userInput.Contains(option));
        }

        public static bool ValidateUserInputIsNotEmpty(string userInput)
        {
            return !string.IsNullOrWhiteSpace(userInput);
        }

        public static bool TryGetDateFromUserInput(string userInput, out DateTime? userRequestedDate)
        {
            DateTime datetimeDump = default(DateTime);
            var managedToRecognize = GetDateOrTimeFromUserInput(userInput, UserDateTimeValueOptions.Date, out datetimeDump);
            userRequestedDate = datetimeDump;

            // the returned formate is in mm/dd/yyyy not in dd/mm/yyyy so we need to switch day and month if possible
            if (TryReplaceDayWithMonthInRecognizedDate(userRequestedDate, out datetimeDump))
            {
                userRequestedDate = datetimeDump;
            }

            return managedToRecognize;
        }

        public static bool TryGetTimeFromUserInput(string userInput, out TimeSpan? userRequestedTime)
        {
            DateTime datetimeDump = default(DateTime);
            var managedToRecognize = GetDateOrTimeFromUserInput(userInput, UserDateTimeValueOptions.Time, out datetimeDump);
            userRequestedTime = datetimeDump.TimeOfDay;
            return managedToRecognize;
        }

        public static bool TryReplaceDayWithMonthInRecognizedDate(DateTime? userRequestedDate, out DateTime result)
        {
            result = default(DateTime);
            if (userRequestedDate.HasValue)
            {
                if (userRequestedDate.Value.Day < 13)
                {
                    result = new DateTime(userRequestedDate.Value.Year, userRequestedDate.Value.Day, userRequestedDate.Value.Month);
                    return true;
                }
            }

            return false;
        }

        private static bool GetDateOrTimeFromUserInput(string userInput, UserDateTimeValueOptions requestedValue, out DateTime userRequestedDateOrTime)
        {
            userRequestedDateOrTime = default(DateTime);
            var managedToRecognize = false;
            string requestedValueKey = GetRecognitionKeyBasedOnRequestedValue(requestedValue);
            var culture = Culture.English;
            var dateTimeRecognitionResult = DateTimeRecognizer.RecognizeDateTime(userInput, culture);
            if (dateTimeRecognitionResult.Any(entity => entity.TypeName == requestedValueKey))
            {
                var recognizedDateResolution = dateTimeRecognitionResult
                    .First(entity => entity.TypeName == requestedValueKey)
                    .Resolution["values"] as List<Dictionary<string, string>>;

                DateTime dateTimeDump = default(DateTime);
                if (recognizedDateResolution.Any(foundDate => DateTime.TryParse(foundDate["value"], out dateTimeDump)))
                {
                    managedToRecognize = true;
                    userRequestedDateOrTime = dateTimeDump;
                }
            }

            return managedToRecognize;
        }

        private static string GetRecognitionKeyBasedOnRequestedValue(UserDateTimeValueOptions requestedValue)
        {
            var requestedValueKey = string.Empty;
            switch (requestedValue)
            {
                case UserDateTimeValueOptions.Date:
                    requestedValueKey = "datetimeV2.date";
                    break;
                case UserDateTimeValueOptions.Time:
                    requestedValueKey = "datetimeV2.time";
                    break;
                default:
                    requestedValueKey = "datetimeV2.date";
                    break;
            }

            return requestedValueKey;
        }
    }

    //TODO:- find a better name for this enum
    internal enum UserDateTimeValueOptions
    {
        Date,
        Time
    }
}
