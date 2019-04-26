namespace BookingAMaintenanceService.Dialogs
{
    using BookingAMaintenanceService.Dialogs.Definitions;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class BookingAMaintenanceServiceDialog : IStatefulDialog<BookingAMaintenanceServiceStatuses>
    {
        public BookingAMaintenanceServiceDialog(ConversationData conversationData, UserData userProfile) 
            : base(conversationData, userProfile)
        {
        }

        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (conversationData.ServiceBookingForm.RequestedService == null)
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.SelectingTheRequestedMaintenanceService);
            }
            else if (conversationData.ServiceBookingForm.RequiredServiceDescription == null)
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.GettingTheRequiredServiceDescription);
            }
            else if (conversationData.ServiceBookingForm.DeliveryLocation == null)
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.GettingTheServiceDeliveryLocation);
            }
            else if (!conversationData.ServiceBookingForm.IsServiceDeliveryTimeSet)
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.GettingTheRequiredServiceTime);
            }
            else
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.ConfirmingServiceInfo);
            }
        }

        protected override IDialog GetSuitableSubDialogBasedOnStatus()
        {
            if (!this.currentDialogStatusHasValue)
            {
                SetCurrentDialogStatus(BookingAMaintenanceServiceStatuses.SelectingTheRequestedMaintenanceService);
            }

            switch (this.currentDialogStatus)
            {
                case BookingAMaintenanceServiceStatuses.SelectingTheRequestedMaintenanceService:
                    return new SelectingRequestedMaintenanceServiceDialog(conversationData, userProfile);
                case BookingAMaintenanceServiceStatuses.GettingTheRequiredServiceDescription:
                    return new GettingRequiredServiceDescriptionDialog(conversationData, userProfile);
                case BookingAMaintenanceServiceStatuses.GettingTheServiceDeliveryLocation:
                    return new GettingServiceDeliveryLocationDialog(conversationData, userProfile);
                case BookingAMaintenanceServiceStatuses.GettingTheRequiredServiceTime:
                    return new GettingRequiredServiceDeliveryTimeDialog(conversationData, userProfile);
                case BookingAMaintenanceServiceStatuses.ConfirmingServiceInfo:
                    return new ConfirmingServiceInfoDialog(conversationData, userProfile);
                default:
                    return new SelectingRequestedMaintenanceServiceDialog(conversationData, userProfile);
            }
        }
    }
}
