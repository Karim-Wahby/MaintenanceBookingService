namespace MaintenanceBookingService.Bot.Dialogs.Constants
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Models;
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
            Options = new MessageOption()
            {
                Arabic = new string[] { "استعلام", "حجز" },
                English = new string[] { "Make a reservation", "Check Request Status" }
            }
        };


        public static HashSet<string> ReservationPossibleSelectionValues = new HashSet<string>()
        {
            "حجز",
            "book",
            "new",
            "make a reservation",
            "1"
        };

        public static HashSet<string> CheckingStatusPossibleSelectionValues = new HashSet<string>()
        {
            "استعلام",
            "check",
            "status",
            "check Request Status",
            "2"
        };
    }
}
