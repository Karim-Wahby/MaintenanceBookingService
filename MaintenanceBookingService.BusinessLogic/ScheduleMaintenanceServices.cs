namespace MaintenanceBookingService.BusinessLogic
{
    using MaintenanceBookingService.DAL;
    using MaintenanceBookingService.Definitions;
    using System;
    using System.Collections.Generic;

    public static class ScheduleMaintenanceServices
    {
        public static bool AddNewServiceRequest(BookingRequest requestInfo)
        {
            return MaintenanceServiceStore.AddNewServiceRequest(requestInfo);
        }

        public static bool ApproveRequest(string requestId)
        {
            // we could add logic to keep track of available slots to book a service.
            return MaintenanceServiceStore.ApproveRequestIfFound(requestId);
        }
    }
}
