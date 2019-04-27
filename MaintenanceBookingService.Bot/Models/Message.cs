namespace MaintenanceBookingService.Models
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

        public MessageOption Options { get; set; }

        public string CombineLanguageValues(string separator = null)
        {
            if (separator == null)
            {
                separator = DefaultStringSeparator;
            }

            return English + separator + Arabic;
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
                Options = MessageOption.CombineOptions(firstMessage.Options, secondMessage.Options),
            };
        }

    }
}
