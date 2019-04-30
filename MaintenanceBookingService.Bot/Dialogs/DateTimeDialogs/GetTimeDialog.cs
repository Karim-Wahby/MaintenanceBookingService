namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;

    public class GetTimeDialog : IStatefulDialog<GettingTimeStatuses>
    {
        public GetTimeDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        protected override void DetermineWhatShouldtheCurrentDialogStatusBe()
        {
            if (!this.ConversationData.ServiceBookingForm.FailedToRecognizeProvidedTime)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.TryingGettingFullTime);
            }
            else if (!this.ConversationData.ServiceBookingForm.Hour.HasValue)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.GettingHour);
            }
            else if (!this.ConversationData.ServiceBookingForm.Minutes.HasValue)
            {
                this.SetCurrentDialogStatus(GettingTimeStatuses.GettingMinute);
            }
            else if (string.IsNullOrWhiteSpace(this.ConversationData.ServiceBookingForm.DayOrNight))
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
                    return new TryGettingFullTimeDialog(ConversationData, UserProfile);
                case GettingTimeStatuses.GettingHour:
                    return new GetHourDialog(ConversationData, UserProfile);
                case GettingTimeStatuses.GettingMinute:
                    return new GetMinuteDialog(ConversationData, UserProfile);
                case GettingTimeStatuses.GettingPartOfDay:
                    return new GetPartOfDayDialog(ConversationData, UserProfile);
                default:
                    return new TryGettingFullTimeDialog(ConversationData, UserProfile);
            }
        }
    }
}