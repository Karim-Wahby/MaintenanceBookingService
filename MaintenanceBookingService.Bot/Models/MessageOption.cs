namespace MaintenanceBookingService.Bot.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MessageOption
    {
        public string[] Arabic { get; set; } = null;

        public string[] English { get; set; } = null;

        public MessageOption Combine(MessageOption otherOptions)
        {
            return CombineOptions(this, otherOptions);
        }

        public string[] CombineLanguagesOptions()
        {
            return CombineOptions(this.Arabic, this.English);
        }

        public static MessageOption CombineOptions(MessageOption firstOptions, MessageOption secondOptions)
        {
            MessageOption newOptions = null;
            if (firstOptions != null && secondOptions != null)
            {
                newOptions = new MessageOption
                {
                    Arabic = CombineOptions(firstOptions.Arabic, secondOptions.Arabic),
                    English = CombineOptions(firstOptions.English, secondOptions.English)
                };
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
