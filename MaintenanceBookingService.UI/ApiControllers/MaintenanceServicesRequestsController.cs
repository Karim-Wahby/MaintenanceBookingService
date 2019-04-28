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
    using System.Threading.Tasks;
    using System.Web.Http;

    public class MaintenanceServicesRequestsController : ApiController
    {
        [Route("api/MaintenanceServicesRequests/")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAllRequests()
        {
            return ScheduleMaintenanceServices.GetAllPendingRequests();
        }

        [Route("api/MaintenanceServicesRequests/UserRequests/{userId}")]
        [HttpGet]
        public IEnumerable<BookingRequest> GetAddUserRequests(string userId)
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
        public bool ApproveRequest(string id)
        {
            return ScheduleMaintenanceServices.ApproveRequest(id);
        }
    }
}