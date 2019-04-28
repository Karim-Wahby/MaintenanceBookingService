namespace MaintenanceBookingService.Bot.Dialogs.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum GettingDateStatuses
    {
        TryingGettingFullDate,
        GettingDay,
        GettingMonth,
        GettingYear,
    }
}
