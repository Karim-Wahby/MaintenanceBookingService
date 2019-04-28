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
        private static readonly HttpClient httpClient = new HttpClient();

        [Route("api/MaintenanceServicesRequests/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllRequests()
        {
            return ScheduleMaintenanceServices.GetAllPendingRequests();
        }

        [Route("api/MaintenanceServicesRequests/UserRequests/{userId}")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetUserRequests(string userId)
        {
            return ScheduleMaintenanceServices.GetRequestWithUserId(userId);
        }

        [Route("api/MaintenanceServicesRequests/PendingApproval/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllPendingApprovalRequests()
        {
            return ScheduleMaintenanceServices.GetAllPendingRequests();
        }

        [Route("api/MaintenanceServicesRequests/{id}")]
        public BookingRequest GetRequestWithId(string id)
        {
            return ScheduleMaintenanceServices.GetRequestWithId(id);
        }

        [Route("api/MaintenanceServicesRequests/AddRequest")]
        [HttpPost]
        public async Task<bool> AddRequest()
        {
            var request = await Request.Content.ReadAsAsync<BookingRequest>();
            return ScheduleMaintenanceServices.AddNewServiceRequest(request);
        }

        [Route("api/MaintenanceServicesRequests/ApproveRequest/{id}")]
        [HttpGet]
        public async Task<bool> ApproveRequestAsync(string id)
        {
            var approvedRequest = ScheduleMaintenanceServices.ApproveRequest(id);
            if (approvedRequest != null)
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(approvedRequest), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:3978/api/UpdateCustomerWithHisRequestStatus/", stringContent);
                return response.StatusCode == HttpStatusCode.OK;
            }

            return false;
        }
    }
}