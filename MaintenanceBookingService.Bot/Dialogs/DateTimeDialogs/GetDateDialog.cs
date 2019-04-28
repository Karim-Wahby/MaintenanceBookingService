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
            if (!this.conversationData.ServiceBookingForm.FailedToRecognizeProvidedDate)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.TryingGettingFullDate);
            }
            else if (!this.conversationData.ServiceBookingForm.Day.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingDay);
            }
            else if (!this.conversationData.ServiceBookingForm.Month.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingMonth);
            }
            else if (!this.conversationData.ServiceBookingForm.Year.HasValue)
            {
                this.SetCurrentDialogStatus(GettingDateStatuses.GettingYear);
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
                    return new DateTimeDialogs.TryGettingFullDateDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingDay:
                    return new DateTimeDialogs.GetDayDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingMonth:
                    return new DateTimeDialogs.GetMonthDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingYear:
                    return new DateTimeDialogs.GetYearDialog(conversationData, userProfile);
                default:
                    return new DateTimeDialogs.TryGettingFullDateDialog(conversationData, userProfile);
            }
        }
    }
}