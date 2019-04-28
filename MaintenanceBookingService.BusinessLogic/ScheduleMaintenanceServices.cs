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

        public static BookingRequest ApproveRequest(string requestId)
        {
            // we could add logic to keep track of available slots to book a service.
            return MaintenanceServiceStore.ApproveRequestIfFound(requestId);
        }

        public static IEnumerable<BookingRequest> GetAllPendingRequests()
        {
            return MaintenanceServiceStore.GetRequestsWithStatus(RequestStatuses.PendingApproval);
        }

        public static IEnumerable<BookingRequest> GetAllApprovedRequests()
        {
            return MaintenanceServiceStore.GetRequestsWithStatus(RequestStatuses.ApprovedAndWaitingDelivery);
        }

        public static IEnumerable<BookingRequest> GetAllDeliveredRequests()
        {
            return MaintenanceServiceStore.GetRequestsWithStatus(RequestStatuses.Delivered);
        }

        public static IEnumerable<BookingRequest> GetRequestWithUserId(string userId)
        {
            return MaintenanceServiceStore.GetRequestWithUserId(userId);
        }

        public static BookingRequest GetRequestWithId(string requestId)
        {
            return MaintenanceServiceStore.GetRequestWithId(requestId);
        }
    }
}
