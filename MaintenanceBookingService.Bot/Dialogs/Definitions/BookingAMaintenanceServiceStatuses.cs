namespace MaintenanceBookingService.Dialogs.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public enum MaintenanceBookingServiceStatuses
    {
        SelectingTheRequestedMaintenanceService,
        GettingTheRequiredServiceDescription,
        GettingTheServiceDeliveryLocation,
        GettingTheRequiredServiceTime,
        ConfirmingServiceInfo,
    }
}
