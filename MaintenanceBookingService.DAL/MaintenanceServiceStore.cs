namespace MaintenanceBookingService.DAL
{
    using MaintenanceBookingService.Definitions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MaintenanceServiceStore
    {
        /// <summary>
        /// this will work as our database for now ^_^
        /// no need to spend more money on resources
        /// </summary>
        private static Dictionary<RequestStatuses, Dictionary<string, BookingRequest>> ServiceRequests;

        public static uint bookedServicesCounter;

        static MaintenanceServiceStore()
        {
            bookedServicesCounter = 0;
            ServiceRequests = new Dictionary<RequestStatuses, Dictionary<string, BookingRequest>>();
            ServiceRequests.Add(RequestStatuses.PendingApproval, new Dictionary<string, BookingRequest>());
            ServiceRequests.Add(RequestStatuses.ApprovedAndWaitingDelivery, new Dictionary<string, BookingRequest>());
            ServiceRequests.Add(RequestStatuses.Delivered, new Dictionary<string, BookingRequest>());
        }
        
        public static bool AddNewServiceRequest(BookingRequest requestInfo)
        {
            if (string.IsNullOrWhiteSpace(requestInfo.Id))
            {
                requestInfo.SetRequestId(GetNewServiceId());
            }

            if (ServiceRequests[RequestStatuses.PendingApproval].ContainsKey(requestInfo.Id))
            {
                ServiceRequests[RequestStatuses.PendingApproval][requestInfo.Id] = requestInfo;
                return false;
            }
            else
            {
                ServiceRequests[RequestStatuses.PendingApproval].Add(requestInfo.Id, requestInfo);
                return true;
            }
        }

        public static bool UpdateNewRequestInfo(BookingRequest requestInfo)
        {
            if (requestInfo == null)
            {
                throw new ArgumentNullException(nameof(requestInfo));
            }

            if (string.IsNullOrWhiteSpace(requestInfo.Id))
            {
                throw new ArgumentNullException("request have an empty Id Value.", nameof(requestInfo));
            }

            ServiceRequests[GetRequestStatus(requestInfo.Id)][requestInfo.Id] = requestInfo;
            return true;
        }

        public static bool ApproveRequestIfFound(string requestId)
        {
            if (ServiceRequests[RequestStatuses.PendingApproval].ContainsKey(requestId))
            {
                var approvedRequest = ServiceRequests[RequestStatuses.PendingApproval][requestId];
                ServiceRequests[RequestStatuses.PendingApproval].Remove(requestId);

                if (ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery].ContainsKey(requestId))
                {
                    ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery][requestId] = approvedRequest;
                }
                else
                {
                    ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery].Add(requestId, approvedRequest);
                }

                return true;
            }

            return false;
        }

        public static bool FinalizeRequestIfFound(string requestId)
        {
            if (ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery].ContainsKey(requestId))
            {
                var deliveredRequest = ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery][requestId];
                ServiceRequests[RequestStatuses.ApprovedAndWaitingDelivery].Remove(requestId);

                if (ServiceRequests[RequestStatuses.Delivered].ContainsKey(requestId))
                {
                    ServiceRequests[RequestStatuses.Delivered][requestId] = deliveredRequest;
                }
                else
                {
                    ServiceRequests[RequestStatuses.Delivered].Add(requestId, deliveredRequest);
                }

                return true;
            }

            return false;
        }

        public static RequestStatuses GetRequestStatus(string requestId)
        {
            try
            {
                return ServiceRequests.First(keyValuePair => keyValuePair.Value.ContainsKey(requestId)).Key;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("no request was found with the required Key", nameof(requestId));
            }
        }

        public static IEnumerable<BookingRequest> GetRequestsWithStatus(RequestStatuses status)
        {
            return ServiceRequests[status].Values;
        }

        private static string GetNewServiceId()
        {
            return (bookedServicesCounter++).ToString();
        }
    }
}
