namespace MaintenanceBookingService.Dialogs
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Models;

    public class GetTimeDialog : IStatefulDialog<GettingTimeStatuses>
    {
        public GetTimeDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (!this.conversationData.ServiceBookingForm.FailedToRecognizeProvidedTime)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.TryingGettingFullTime);
            }
            else if (!this.conversationData.ServiceBookingForm.Hour.HasValue)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.GettingHour);
            }
            else if (!this.conversationData.ServiceBookingForm.Minutes.HasValue)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.GettingMinute);
            }
            else if (string.IsNullOrWhiteSpace(this.conversationData.ServiceBookingForm.DayOrNight))
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.GettingPartOfDay);
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
                case GettingTimeStatuses.TryingGettingFullTime:
                    return new TryGettingFullTimeDialog(conversationData, userProfile);
                case GettingTimeStatuses.GettingHour:
                    return new GetHourDialog(conversationData, userProfile);
                case GettingTimeStatuses.GettingMinute:
                    return new GetMinuteDialog(conversationData, userProfile);
                case GettingTimeStatuses.GettingPartOfDay:
                    return new GetPartOfDayDialog(conversationData, userProfile);
                default:
                    return new TryGettingFullTimeDialog(conversationData, userProfile);
            }
        }
    }
}