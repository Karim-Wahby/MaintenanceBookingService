namespace MaintenanceBookingService.Bot.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Definitions;
    using Microsoft.AspNetCore.Mvc;

    public class UpdateCustomerWithHisRequestStatusController : Controller
    {
        // GET: api/<controller>
        [HttpPost]
        [Route("api/UpdateCustomerWithHisRequestStatus/")]
        public async Task RequestApprovedAsync([FromBody]BookingRequest request)
        {
        }
    }
}


