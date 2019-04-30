namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;

    public class GetDateDialog : IStatefulDialog<GettingDateStatuses>
    {
        public GetDateDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (!this.ConversationData.ServiceBookingForm.FailedToRecognizeProvidedDate)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.TryingGettingFullDate);
            }
            else if (!this.ConversationData.ServiceBookingForm.Year.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingYear);
            }
            else if (!this.ConversationData.ServiceBookingForm.Month.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingMonth);
            }
            else if (!this.ConversationData.ServiceBookingForm.Day.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingDay);
            }
            else
            {
                // TODO:- Telimetry for this invalid state
            }
        }

        protected override IDialog GetSuitableSubDialogBasedOnStatus()
        {
            switch (this.currentDialogStatus)
            {
                case GettingDateStatuses.TryingGettingFullDate:
                    return new DateTimeDialogs.TryGettingFullDateDialog(ConversationData, UserProfile);
                case GettingDateStatuses.GettingDay:
                    return new DateTimeDialogs.GetDayDialog(ConversationData, UserProfile);
                case GettingDateStatuses.GettingMonth:
                    return new DateTimeDialogs.GetMonthDialog(ConversationData, UserProfile);
                case GettingDateStatuses.GettingYear:
                    return new DateTimeDialogs.GetYearDialog(ConversationData, UserProfile);
                default:
                    return new DateTimeDialogs.TryGettingFullDateDialog(ConversationData, UserProfile);
            }
        }
    }
}