namespace BookingAMaintenanceService.Dialogs.Constants
{
    using BookingAMaintenanceService.Dialogs.Definitions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class UserIntentSelection
    {
        public static Message IntentSelection = new Message()
        {
            Arabic = $"كيف استطيع مساعدتك اليوم:{Environment.NewLine}" +
                        $"1- حجز خدمه صيانه{Environment.NewLine}" +
                        $"2- تفقد الحاله الحاليه لخدماتك{Environment.NewLine}",
            English = $"How Can I Help You Today:{Environment.NewLine}" +
                        $"1- Book a new Maintenance Service {Environment.NewLine}" +
                        $"2- Check your Requests Status{Environment.NewLine}",
            ArabicOptions = new string[] { "استعلام", "حجز" },
            EnglishOptions = new string[] { "Make a reservation", "Check Request Status" }
        };


        public static HashSet<string> ReservationPossibleSelectionValues = new HashSet<string>()
        {
            "حجز",
            "book",
            "new",
            "Make a reservation".ToLower(),
            "1"
        };

        public static HashSet<string> CheckingStatusPossibleSelectionValues = new HashSet<string>()
        {
            "استعلام",
            "check",
            "status",
            "Check Request Status".ToLower(),
            "2"
        };
    }
}
