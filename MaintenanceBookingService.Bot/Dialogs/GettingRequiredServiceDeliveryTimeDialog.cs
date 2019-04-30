namespace MaintenanceBookingService.Bot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GettingRequiredServiceDeliveryTimeDialog : IStatefulDialog<GettingServiceDeliveryTimeStatuses>
    {
        public GettingRequiredServiceDeliveryTimeDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }
        
        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (!this.ConversationData.ServiceBookingForm.IsDateSet)
            {
                this.SetCurrentDialogStatus(GettingServiceDeliveryTimeStatuses.GettingDate);
            }
            else if (!this.ConversationData.ServiceBookingForm.IsTimeSet)
            {
                this.SetCurrentDialogStatus(GettingServiceDeliveryTimeStatuses.GettingTime);
            }
            else
            {
                this.SetCurrentDialogStatus(GettingServiceDeliveryTimeStatuses.GettingDate);
            }
        }

        protected override IDialog GetSuitableSubDialogBasedOnStatus()
        {
            switch (this.currentDialogStatus)
            {
                case GettingServiceDeliveryTimeStatuses.GettingDate:
                    return new DateTimeDialogs.GetDateDialog(ConversationData, UserProfile);
                case GettingServiceDeliveryTimeStatuses.GettingTime:
                    return new DateTimeDialogs.GetTimeDialog(ConversationData, UserProfile);
                default:
                    return new DateTimeDialogs.GetDateDialog(ConversationData, UserProfile);
            }
        }
    }
}