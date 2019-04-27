namespace MaintenanceBookingService.Dialogs.Constants
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class LanguageSelection
    {
        private static string askingForpreferredLanguageMessage =
            $"Please Select Your prefered Language:-{Environment.NewLine}" +
                $"أختر اللغه التى تفضلها رجاء{Environment.NewLine}" +
                $"1- English{Environment.NewLine}" +
                $"2- عربى";

        public static Message AskingForpreferredLanguage = new Message()
        {
            IsLanguageIndependent = true,
            English = askingForpreferredLanguageMessage,
            Arabic = askingForpreferredLanguageMessage,
            Options = new MessageOption()
            {
                Arabic = new string[] { "عربى" },
                English = new string[] { "English" }
            }
        };

        public static HashSet<string> ArabicLanguagePossibleSelectionValues = new HashSet<string>()
        {
            "ar",
            "arabic",
            "ع",
            "2"
        };

        public static HashSet<string> EnglishLanguagePossibleSelectionValues = new HashSet<string>()
        {
            "eng",
            "e",
            "1"
        };

        static LanguageSelection()
        {
            foreach (var option in AskingForpreferredLanguage.Options.Arabic)
            {
                ArabicLanguagePossibleSelectionValues.Add(option);
            }

            foreach (var option in AskingForpreferredLanguage.Options.English)
            {
                EnglishLanguagePossibleSelectionValues.Add(option.ToLower());
            }
        }
    }
}
