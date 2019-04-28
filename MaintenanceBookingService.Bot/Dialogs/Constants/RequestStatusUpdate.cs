namespace MaintenanceBookingService.Bot.Dialogs.Constants
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class RequestStatusUpdate
    {
        public static Message ServiceRequestApprovedMessage = new Message()
        {
            English = $"Your Service Request With Id {{0}}: ({{1}}){Environment.NewLine}Has Been Approved",
            Arabic = $"طلبكم رقم {{0}}: ({{1}}){Environment.NewLine} قد تم الموافقه عليه",
        };

        public static Message ServiceRequestDeliveredMessage = new Message()
        {
            English = $"Your Service Request With Id {{0}}: ({{1}}){Environment.NewLine}Has Been Delivered{Environment.NewLine}please rate the delivered service",
            Arabic = $"طلبكم رقم {{0}}: ({{1}}){Environment.NewLine}  قد تم توصيله بنجاح فضلا قم بتقيم الخدمه",
            Options = new MessageOption
            {
                English = new string[] { "Poor", "Fair", "Good", "Very Good", "Excellent" },
                Arabic = new string[] { "سيئة", "متوسطه", "جيدة", "جيدة جدا", "ممتازة" }
            }
        };
    }
}
