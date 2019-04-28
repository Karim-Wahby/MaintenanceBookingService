namespace MaintenanceBookingService.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BookingRequest
    {
        public string Id { get; set; } = string.Empty;

        public ConversationChannelData ConversationChannelData { get; set; } = null;

        public SupportedLanguage UserPreferredLanguage { get; set; }

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
            ConversationChannelData conversationChannelData,
            SupportedLanguage userPreferredLanguage)
            : this(maintenanceServiceNeeded, descriptionOfRequiredService, deliveryLocation, timeOfServiceDelivery, conversationChannelData, userPreferredLanguage)
        {
            this.SetRequestId(identifier);
        }

        public BookingRequest(
            SupportedMaintenanceServices maintenanceServiceNeeded, 
            string descriptionOfRequiredService, 
            string deliveryLocation, 
            DateTime timeOfServiceDelivery,
            ConversationChannelData conversationChannelData,
            SupportedLanguage userPreferredLanguage)
        {
            this.UserPreferredLanguage = userPreferredLanguage;
            this.ConversationChannelData = conversationChannelData ?? throw new ArgumentNullException(nameof(conversationChannelData));
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
