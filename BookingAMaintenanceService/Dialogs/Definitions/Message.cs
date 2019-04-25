namespace BookingAMaintenanceService.Dialogs.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Message
    {
        public static string DefaultStringSeparator = Environment.NewLine;

        public bool IsLanguageIndependent = false;

        public string Arabic { get; set; }

        public string English { get; set; }

        public string[] ArabicOptions { get; set; } = null;

        public string[] EnglishOptions { get; set; } = null;

        public string CombineLanguageValues(string separator = null)
        {
            if (separator == null)
            {
                separator = DefaultStringSeparator;
            }

            return English + separator + Arabic;
        }

        public string[] CombineLanguagesOptions()
        {
            return CombineOptions(ArabicOptions, EnglishOptions);
        }

        public string[] GetLanguagesValuesAsOptions()
        {
            return new string[] { Arabic, English };
        }

        public static Message CombineMessages(Message firstMessage, Message secondMessage, string messageStringSeparator = null)
        {
            if (messageStringSeparator == null)
            {
                messageStringSeparator = DefaultStringSeparator;
            }

            return new Message()
            {
                Arabic = firstMessage.Arabic + messageStringSeparator + secondMessage.Arabic,
                English = firstMessage.English + messageStringSeparator + secondMessage.English,
                ArabicOptions = CombineOptions(firstMessage.ArabicOptions, secondMessage.ArabicOptions),
                EnglishOptions = CombineOptions(firstMessage.EnglishOptions, secondMessage.EnglishOptions)
            };
        }

        private static string[] CombineOptions(string[] firstOptions, string[] secondOptions)
        {
            string[] newOptions = null;
            if (firstOptions != null && secondOptions != null)
            {
                newOptions = firstOptions.Union(secondOptions).Distinct().ToArray();
            }
            else if (firstOptions != null)
            {
                newOptions = firstOptions;
            }
            else
            {
                newOptions = secondOptions;
            }

            return newOptions;
        }
    }
}
