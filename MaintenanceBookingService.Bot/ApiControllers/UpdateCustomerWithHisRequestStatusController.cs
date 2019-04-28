namespace MaintenanceBookingService.Bot.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.ProactiveMessaging;
    using MaintenanceBookingService.Definitions;
    using Microsoft.AspNetCore.Mvc;

    public class UpdateCustomerWithHisRequestStatusController : Controller
    {
        // GET: api/<controller>
        [HttpPost]
        [Route("api/UpdateCustomerWithHisRequestStatus/RequestApproved")]
        public async Task RequestApprovedAsync([FromBody]BookingRequest request)
        {
            var messageOptions = new string[]
                {
                    request.Id,
                    request.DescriptionOfRequiredService
                };

            await AdHocMessage.SendMessageAsync(
                request.ConversationChannelData,
                request.UserPreferredLanguage,
                Dialogs.Constants.RequestStatusUpdate.ServiceRequestApprovedMessage,
                new Models.MessageOption()
                {
                    Arabic = messageOptions,
                    English = messageOptions
                });
        }

        // GET: api/<controller>
        [HttpPost]
        [Route("api/UpdateCustomerWithHisRequestStatus/RequestDelivered")]
        public async Task RequestDeliveredAsync([FromBody]BookingRequest request)
        {
            var messageOptions = new string[]
                {
                    request.Id,
                    request.DescriptionOfRequiredService
                };

            await AdHocMessage.SendMessageAsync(
                request.ConversationChannelData,
                request.UserPreferredLanguage,
                Dialogs.Constants.RequestStatusUpdate.ServiceRequestDeliveredMessage,
                new Models.MessageOption()
                {
                    Arabic = messageOptions,
                    English = messageOptions
                },
                startNewConversation: true);
        }
    }
}


