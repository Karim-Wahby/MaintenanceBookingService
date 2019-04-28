namespace MaintenanceBookingService.Bot.Dialogs.Constants
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Models;
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
    }
}
