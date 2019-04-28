namespace MaintenanceBookingService.UI.ApiControllers
{
    using MaintenanceBookingService.BusinessLogic;
    using MaintenanceBookingService.Definitions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class MaintenanceServicesRequestsController : ApiController
    {
        private static string botUserUpdatingEndPointUrl = "http://localhost:3978/api/UpdateCustomerWithHisRequestStatus/";
        private static readonly HttpClient httpClient = new HttpClient();

        [Route("api/MaintenanceServicesRequests/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllRequests()
        {
            return ScheduleMaintenanceServices.GetAllRequests();
        }

        [Route("api/MaintenanceServicesRequests/PendingApproval/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllPendingApprovalRequests()
        {
            return ScheduleMaintenanceServices.GetAllPendingApprovalRequests();
        }

        [Route("api/MaintenanceServicesRequests/ApprovedRequests/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllApprovedRequests()
        {
            return ScheduleMaintenanceServices.GetAllApprovedRequests();
        }

        [Route("api/MaintenanceServicesRequests/DeliveredRequests/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllDeliveredRequests()
        {
            return ScheduleMaintenanceServices.GetAllDeliveredRequests();
        }

        [Route("api/MaintenanceServicesRequests/UserRequests/{userId}")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetUserRequests(string userId)
        {
            return ScheduleMaintenanceServices.GetRequestWithUserId(userId);
        }
        
        [Route("api/MaintenanceServicesRequests/{id}")]
        public BookingRequest GetRequestWithId(string id)
        {
            return ScheduleMaintenanceServices.GetRequestWithId(id);
        }

        [Route("api/MaintenanceServicesRequests/AddRequest")]
        [HttpPost]
        public async Task<string> AddRequest()
        {
            var request = await Request.Content.ReadAsAsync<BookingRequest>();
            if (ScheduleMaintenanceServices.AddNewRequestPendingApproval(request))
            {
                return request.Id;
            }
            else
            {
                return string.Empty;
            }
        }

        [Route("api/MaintenanceServicesRequests/ApproveRequest/{id}")]
        [HttpGet]
        public async Task<bool> ApproveRequestAsync(string id)
        {
            var approvedRequest = ScheduleMaintenanceServices.ApproveRequest(id);
            if (approvedRequest != null)
            {
                return await UpdateUserWithNewRequestStatus(approvedRequest);
            }

            return false;
        }

        [Route("api/MaintenanceServicesRequests/FinalizeRequest/{id}")]
        [HttpGet]
        public async Task<bool> FinalizeRequestAsync(string id)
        {
            var finalizedRequest = ScheduleMaintenanceServices.FinalizeRequest(id);
            if (finalizedRequest != null)
            {
                return await UpdateUserWithNewRequestStatus(finalizedRequest);
            }

            return false;
        }

        private async Task<bool> UpdateUserWithNewRequestStatus(BookingRequest userRequest)
        {
            if (userRequest != null)
            {
                var endpointToHitInBot = botUserUpdatingEndPointUrl;
                if (userRequest.Status == RequestStatuses.ApprovedAndWaitingDelivery)
                {
                    endpointToHitInBot += "RequestApproved";
                }
                else if (userRequest.Status == RequestStatuses.Delivered)
                {
                    endpointToHitInBot += "RequestDelivered";
                }
                else
                {
                    return true;
                }

                var requestAsStringContent = new StringContent(JsonConvert.SerializeObject(userRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(endpointToHitInBot, requestAsStringContent);
                return response.StatusCode == HttpStatusCode.OK;
            }

            return false;
        }
    }
}