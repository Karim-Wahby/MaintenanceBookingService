namespace MaintenanceBookingService.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BookingRequest
    {
        public string Id { get; private set; } = string.Empty;

        public SupportedMaintenanceServices MaintenanceServiceNeeded { get; private set; }

        public string DescriptionOfRequiredService { get; private set; }

        public string DeliveryLocation { get; private set; }

        public DateTime TimeOfServiceDelivery { get; private set; }

        public BookingRequest(
            string identifier,
            SupportedMaintenanceServices maintenanceServiceNeeded,
            string descriptionOfRequiredService,
            string deliveryLocation,
            DateTime timeOfServiceDelivery)
            : this(maintenanceServiceNeeded, descriptionOfRequiredService, deliveryLocation, timeOfServiceDelivery)
        {
            this.SetRequestId(identifier);
        }

        public BookingRequest(
            SupportedMaintenanceServices maintenanceServiceNeeded, 
            string descriptionOfRequiredService, 
            string deliveryLocation, 
            DateTime timeOfServiceDelivery)
        {
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
