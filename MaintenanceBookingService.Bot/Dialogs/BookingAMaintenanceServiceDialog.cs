namespace MaintenanceBookingService.Bot.Dialogs
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
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
            if (ConversationData.ServiceBookingForm.RequestedService == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.SelectingTheRequestedMaintenanceService);
            }
            else if (ConversationData.ServiceBookingForm.RequiredServiceDescription == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.GettingTheRequiredServiceDescription);
            }
            else if (ConversationData.ServiceBookingForm.DeliveryLocation == null)
            {
                SetCurrentDialogStatus(MaintenanceBookingServiceStatuses.GettingTheServiceDeliveryLocation);
            }
            else if (!ConversationData.ServiceBookingForm.IsServiceDeliveryTimeSet)
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
                    return new SelectingRequestedMaintenanceServiceDialog(ConversationData, UserProfile);
                case MaintenanceBookingServiceStatuses.GettingTheRequiredServiceDescription:
                    return new GettingRequiredServiceDescriptionDialog(ConversationData, UserProfile);
                case MaintenanceBookingServiceStatuses.GettingTheServiceDeliveryLocation:
                    return new GettingServiceDeliveryLocationDialog(ConversationData, UserProfile);
                case MaintenanceBookingServiceStatuses.GettingTheRequiredServiceTime:
                    return new GettingRequiredServiceDeliveryTimeDialog(ConversationData, UserProfile);
                case MaintenanceBookingServiceStatuses.ConfirmingServiceInfo:
                    return new ConfirmingServiceInfoDialog(ConversationData, UserProfile);
                default:
                    return new SelectingRequestedMaintenanceServiceDialog(ConversationData, UserProfile);
            }
        }
    }
}
