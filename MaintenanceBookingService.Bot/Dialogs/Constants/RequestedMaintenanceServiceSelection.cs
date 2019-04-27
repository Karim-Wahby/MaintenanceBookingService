namespace MaintenanceBookingService.Dialogs.Constants
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class RequestedMaintenanceServiceSelection
    {
        public static Message AskingForRequiredService = new Message()
        {
            English = $"Please Select the maintenance service you need :-{Environment.NewLine}" +
                            $"1- Carpentry (including lock and key services){Environment.NewLine}" +
                            $"2- Electrical maintenance (including refrigeration, alarms and controls, and lighting services){Environment.NewLine}" +
                            $"3- Plumbing services{Environment.NewLine}" +
                            $"4- Air conditioning maintenance{Environment.NewLine}" +
                            $"5- Painting services{Environment.NewLine}" +
                            $"6- Cleaning",
            Arabic = $"فصلا اختر خدمه الصيانه المطلوبه:-{Environment.NewLine}" +
                            $"1- اعمال خشبيه (نجاره) و هذا يضم الكوالين و المفاتيح / {Environment.NewLine}" +
                            $"2- كهرباء, و هذا يتضمن اعمال الثلاجات و الغسالات و خلافه{Environment.NewLine}" +
                            $"3- خدمات سباكه{Environment.NewLine}" +
                            $"4- صيانه مكيف الهواء{Environment.NewLine}" +
                            $"5- خدمات نقاشه{Environment.NewLine}" +
                            $"6- تنظيف",
            Options = new MessageOption()
            {
                Arabic = new string[] { "اعمال خشبيه", "كهرباء", "سباكه", "صيانه مكيف الهواء", "خدمات نقاشه", "تنظيف" },
                English = new string[] { "Carpentry", "Electrical maintenance", "Plumbing", "Air conditioning", "Painting", "Cleaning" }
            }
        };

        public static HashSet<string> CarpentryPossibleValues = new HashSet<string> { "1", "carpentry", "اعمال خشبيه" };
        public static HashSet<string> ElectricalMaintenancePossibleValues = new HashSet<string> { "2", "electrical maintenance", "كهرباء" };
        public static HashSet<string> PlumbingPossibleValues = new HashSet<string> { "3", "plumbing", "سباكه" };
        public static HashSet<string> AirConditioningPossibleValues = new HashSet<string> { "4", "air", "air conditioning", "صيانه مكيف الهواء" };
        public static HashSet<string> PaintingPossibleValues = new HashSet<string> { "5", "painting", "خدمات نقاشه" };
        public static HashSet<string> CleaningPossibleValues = new HashSet<string> { "6", "cleaning", "تنظيف" };
    }
}
