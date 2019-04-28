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

        public static IEnumerable<BookingRequest> GetRequestWithUserId(string userId)
        {
            return ServiceRequests.SelectMany(requestsWithStatuses => requestsWithStatuses.Value.Where(request => request.Value.ConversationChannelData.UserId == userId).Select(request => request.Value));
        }

        public static BookingRequest GetRequestWithId(string requestId)
        {
            return ServiceRequests[GetRequestStatus(requestId)][requestId];
        }

        public static bool AddNewRequestPendingApproval(BookingRequest requestInfo)
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
                requestInfo.Status = RequestStatuses.PendingApproval;
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

        public static BookingRequest ChangeRequestStateIfFound(string requestId, RequestStatuses newStatus)
        {
            try
            {
                var currentRequestStatus = GetRequestStatus(requestId);
                var currentRequest = ServiceRequests[currentRequestStatus][requestId];
                ServiceRequests[currentRequestStatus].Remove(requestId);

                if (ServiceRequests[newStatus].ContainsKey(requestId))
                {
                    ServiceRequests[newStatus][requestId] = currentRequest;
                }
                else
                {
                    ServiceRequests[newStatus].Add(requestId, currentRequest);
                }

                currentRequest.Status = newStatus;
                return currentRequest;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
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

        public static IEnumerable<BookingRequest> GetAllRequests()
        {
            return ServiceRequests.SelectMany(requestsWithStatus => requestsWithStatus.Value).Select(request => request.Value);
        }

        private static string GetNewServiceId()
        {
            return (bookedServicesCounter++).ToString();
        }
    }
}
