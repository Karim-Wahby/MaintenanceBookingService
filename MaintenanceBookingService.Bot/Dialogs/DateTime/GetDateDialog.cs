namespace MaintenanceBookingService.Dialogs
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Models;

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
                    return new TryGettingFullDateDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingDay:
                    return new GetDayDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingMonth:
                    return new GetMonthDialog(conversationData, userProfile);
                case GettingDateStatuses.GettingYear:
                    return new GetYearDialog(conversationData, userProfile);
                default:
                    return new TryGettingFullDateDialog(conversationData, userProfile);
            }
        }
    }
}