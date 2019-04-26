namespace BookingAMaintenanceService.Dialogs.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum GettingTimeStatuses
    {
        TryingGettingFullTime,
        GettingHour,
        GettingMinute,
        GettingPartOfDay,
    }
}
