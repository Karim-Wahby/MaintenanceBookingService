namespace MaintenanceBookingService.Dialogs
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MaintenanceBookingServiceDialog : IStatefulDialog<MaintenanceBookingServiceStatuses>
    {
        public MaintenanceBookingServiceDialog(ConversationData conversationData, UserData userProfile) 
            : base(conversationData, userProfile)
        {
        }

        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (conversationData.ServiceBookingForm.RequestedService == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.SelectingTheRequestedMaintenanceService);
            }
            else if (conversationData.ServiceBookingForm.RequiredServiceDescription == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.GettingTheRequiredServiceDescription);
            }
            else if (conversationData.ServiceBookingForm.DeliveryLocation == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.GettingTheServiceDeliveryLocation);
            }
            else if (!conversationData.ServiceBookingForm.IsServiceDeliveryTimeSet)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.GettingTheRequiredServiceTime);
            }
            else
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.ConfirmingServiceInfo);
            }
        }

        protected override IDialog GetSuitableSubDialogBasedOnStatus()
        {
            if (!this.currentDialogStatusHasValue)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.SelectingTheRequestedMaintenanceService);
            }

            switch (this.currentDialogStatus)
            {
                case MaintenanceBookingServiceStatuses.SelectingTheRequestedMaintenanceService:
                    return new SelectingRequestedMaintenanceServiceDialog(conversationData, userProfile);
                case MaintenanceBookingServiceStatuses.GettingTheRequiredServiceDescription:
                    return new GettingRequiredServiceDescriptionDialog(conversationData, userProfile);
                case MaintenanceBookingServiceStatuses.GettingTheServiceDeliveryLocation:
                    return new GettingServiceDeliveryLocationDialog(conversationData, userProfile);
                case MaintenanceBookingServiceStatuses.GettingTheRequiredServiceTime:
                    return new GettingRequiredServiceDeliveryTimeDialog(conversationData, userProfile);
                case MaintenanceBookingServiceStatuses.ConfirmingServiceInfo:
                    return new ConfirmingServiceInfoDialog(conversationData, userProfile);
                default:
                    return new SelectingRequestedMaintenanceServiceDialog(conversationData, userProfile);
            }
        }
    }
}
