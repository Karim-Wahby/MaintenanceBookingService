namespace MaintenanceBookingService.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BookingRequest
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public string BotId { get; set; } = string.Empty;

        public int? Rating { get; set; } = null;

        public SupportedMaintenanceServices MaintenanceServiceNeeded { get; set; }

        public string DescriptionOfRequiredService { get; set; }

        public string DeliveryLocation { get; set; }

        public RequestStatuses Status { get; set; } = RequestStatuses.NotSpecified;

        public DateTime TimeOfServiceDelivery { get; set; }

        public BookingRequest()
        {
        }

        public BookingRequest(
            string identifier,
            SupportedMaintenanceServices maintenanceServiceNeeded,
            string descriptionOfRequiredService,
            string deliveryLocation,
            DateTime timeOfServiceDelivery,
            string userName,
            string userId,
            string channelId,
            string botId)
            : this(maintenanceServiceNeeded, descriptionOfRequiredService, deliveryLocation, timeOfServiceDelivery, userName, userId, channelId, botId)
        {
            this.SetRequestId(identifier);
        }

        public BookingRequest(
            SupportedMaintenanceServices maintenanceServiceNeeded, 
            string descriptionOfRequiredService, 
            string deliveryLocation, 
            DateTime timeOfServiceDelivery,
            string userName,
            string userId,
            string channelId,
            string botId)
        {
            this.UserName = userName;
            this.UserId = userId;
            this.ChannelId = channelId;
            this.BotId = botId;
            this.DeliveryLocation = deliveryLocation ?? throw new ArgumentNullException(nameof(deliveryLocation));
            this.MaintenanceServiceNeeded = maintenanceServiceNeeded;
            this.TimeOfServiceDelivery = timeOfServiceDelivery;
            this.DescriptionOfRequiredService = descriptionOfRequiredService ?? throw new ArgumentNullException(nameof(descriptionOfRequiredService));
        }

        public void SetRequestId(string newId)
        {
            this.Id = newId;
        }
    }
}
