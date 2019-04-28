namespace MaintenanceBookingService.BusinessLogic
{
    using MaintenanceBookingService.DAL;
    using MaintenanceBookingService.Definitions;
    using System;
    using System.Collections.Generic;

    public static class ScheduleMaintenanceServices
    {
        public static bool AddNewRequestPendingApproval(BookingRequest requestInfo)
        {
            return MaintenanceServiceStore.AddNewRequestPendingApproval(requestInfo);
        }

        public static BookingRequest ApproveRequest(string requestId)
        {
            return MaintenanceServiceStore.ChangeRequestStateIfFound(requestId, RequestStatuses.ApprovedAndWaitingDelivery);
        }

        public static BookingRequest FinalizeRequest(string requestId)
        {
            return MaintenanceServiceStore.ChangeRequestStateIfFound(requestId, RequestStatuses.Delivered);
        }

        public static IEnumerable<BookingRequest> GetAllRequests()
        {
            return MaintenanceServiceStore.GetAllRequests();
        }

        public static IEnumerable<BookingRequest> GetAllPendingApprovalRequests()
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
